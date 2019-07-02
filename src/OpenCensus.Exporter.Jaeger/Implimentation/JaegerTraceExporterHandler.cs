namespace OpenCensus.Exporter.Jaeger.Implimentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;
    using OpenCensus.Trace.Export;
    using Thrift.Protocols;

    public class JaegerTraceExporterHandler : IHandler, IDisposable
    {
        public const string DefaultAgentUdpHost = "localhost";
        public const int DefaultAgentUdpCompactPort = 6831;
        public const int DefaultMaxPacketSize = 65000;


        private readonly JaegerExporterOptions options;
        private readonly JaegerUdpBatcher jaegerAgentUdpBatcher;

        public JaegerTraceExporterHandler(JaegerExporterOptions options)
        {
            ValidateOptions(options);
            InitializeOptions(options);
            this.options = options;
            this.jaegerAgentUdpBatcher = new JaegerUdpBatcher(options);
        }

        public async Task ExportAsync(IEnumerable<ISpanData> spanDataList)
        {
            var jaegerspans = spanDataList.Select(sdl => sdl.ToJaegerSpan());

            foreach(var s in jaegerspans)
            {
                await this.jaegerAgentUdpBatcher.AppendAsync(s, CancellationToken.None);
            }
        }

        private void ValidateOptions(JaegerExporterOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.ServiceName))
            {
                throw new ArgumentNullException("ServiceName", "Service Name is required.");
            }
        }

        private void InitializeOptions(JaegerExporterOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.AgentHost))
            {
                options.AgentHost = DefaultAgentUdpHost;
            }

            if (!options.AgentPort.HasValue)
            {
                options.AgentPort = DefaultAgentUdpCompactPort;
            }

            if (!options.MaxPacketSize.HasValue)
            {
                options.MaxPacketSize = DefaultMaxPacketSize;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.jaegerAgentUdpBatcher.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~JaegerTraceExporterHandler()
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
        #endregion
    }
}
