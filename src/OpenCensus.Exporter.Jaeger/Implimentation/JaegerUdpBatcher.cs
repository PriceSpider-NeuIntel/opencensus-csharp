
namespace OpenCensus.Exporter.Jaeger.Implimentation
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Thrift.Protocols;

    public class JaegerUdpBatcher : IDisposable
    {
        private const int DefaultMaxPacketSize = 65000;
        private readonly int? maxPacketSize;
        private readonly TCompactProtocol.Factory ProtocolFactory;
        private readonly JaegerThriftClientTransport clientTransport;
        private readonly JaegerThriftClient thriftClient;
        private readonly Process process;
        private readonly int processByteSize;
        private readonly List<JaegerSpan> currentBatch = new List<JaegerSpan>();

        private int batchByteSize;

        private bool disposedValue = false; // To detect redundant calls

        public JaegerUdpBatcher(JaegerExporterOptions options)
        {
            this.maxPacketSize = options.MaxPacketSize == 0 ? DefaultMaxPacketSize : options.MaxPacketSize;
            this.ProtocolFactory = new TCompactProtocol.Factory();
            this.clientTransport = new JaegerThriftClientTransport(options.AgentHost, options.AgentPort.Value);
            this.thriftClient = new JaegerThriftClient(this.ProtocolFactory.GetProtocol(this.clientTransport));
            this.process = new Process(options.ServiceName);
            this.processByteSize = GetSize(this.process);
            this.batchByteSize = this.processByteSize;
        }

        public async Task<int> AppendAsync(JaegerSpan span, CancellationToken cancellationToken)
        {
            int spanSize = GetSize(span);
            if (spanSize > this.maxPacketSize)
            {
                throw new JaegerExporterException($"ThriftSender received a span that was too large, size = {spanSize}, max = {this.maxPacketSize}", null);
            }

            this.batchByteSize += spanSize;
            if (this.batchByteSize <= this.maxPacketSize)
            {
                this.currentBatch.Add(span);
                if (this.batchByteSize < this.maxPacketSize)
                {
                    return 0;
                }
                return await FlushAsync(cancellationToken).ConfigureAwait(false);
            }

            int n;
            try
            {
                n = await FlushAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (JaegerExporterException ex)
            {
                // +1 for the span not submitted in the buffer above
                throw new JaegerExporterException(ex.Message, ex);
            }

            this.currentBatch.Add(span);
            this.batchByteSize = this.processByteSize + spanSize;
            return n;
        }

        protected async Task SendAsync(Process process, List<JaegerSpan> spans, CancellationToken cancellationToken)
        {
            try
            {
                var batch = new Batch(process, spans);
                await this.thriftClient.EmitBatchAsync(batch, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new JaegerExporterException($"Could not send {spans.Count} spans", ex);
            }
        }

        public async Task<int> FlushAsync(CancellationToken cancellationToken)
        {
            if (this.currentBatch.Count == 0)
            {
                return 0;
            }

            int n = this.currentBatch.Count;
            try
            {
                await SendAsync(this.process, currentBatch, cancellationToken).ConfigureAwait(false);
            }
            catch (JaegerExporterException ex)
            {
                throw new JaegerExporterException("Failed to flush spans.", ex);
            }
            finally
            {
                this.currentBatch.Clear();
                this.batchByteSize = this.processByteSize;
            }
            return n;
        }

        public virtual Task<int> CloseAsync(CancellationToken cancellationToken)
        {
            return FlushAsync(cancellationToken);
        }

        private int GetSize(TAbstractBase thriftBase)
        {
            using (var memoryTransport = new InMemoryTransport())
            {
                thriftBase.WriteAsync(ProtocolFactory.GetProtocol(memoryTransport), CancellationToken.None).GetAwaiter().GetResult();
                return memoryTransport.GetBuffer().Length;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.FlushAsync(CancellationToken.None).Wait();
                    this.thriftClient.Dispose();
                    this.clientTransport.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                this.disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~JaegerUdpBatcher()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
    }
}
