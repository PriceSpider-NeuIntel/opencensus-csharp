namespace OpenCensus.Exporter.Jaeger.Implimentation
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Thrift;
    using Thrift.Protocols;
    using Thrift.Protocols.Entities;

    public class JaegerThriftClient : TBaseClient, IDisposable
    {
        public JaegerThriftClient(TProtocol protocol)
            : this(protocol, protocol)
        {
        }

        public JaegerThriftClient(TProtocol inputProtocol, TProtocol outputProtocol)
            : base(inputProtocol, outputProtocol)
        {
        }

        public async Task EmitBatchAsync(Batch batch, CancellationToken cancellationToken)
        {
            await OutputProtocol.WriteMessageBeginAsync(new TMessage("emitBatch", TMessageType.Oneway, SeqId), cancellationToken);

            var args = new EmitBatchArgs();
            args.Batch = batch;

            await args.WriteAsync(OutputProtocol, cancellationToken);
            await OutputProtocol.WriteMessageEndAsync(cancellationToken);
            await OutputProtocol.Transport.FlushAsync(cancellationToken);
        }
    }
}
