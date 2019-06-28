namespace OpenCensus.Exporter.Jaeger.Implimentation
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Thrift.Protocols;
    using Thrift.Protocols.Entities;

    public partial class EmitBatchArgs : TAbstractBase
    {
        public EmitBatchArgs()
        {
        }

        public Batch Batch { get; set; }

        public async Task WriteAsync(TProtocol oprot, CancellationToken cancellationToken)
        {
            oprot.IncrementRecursionDepth();
            try
            {
                var struc = new TStruct("emitBatch_args");
                await oprot.WriteStructBeginAsync(struc, cancellationToken);
                var field = new TField();
                if (Batch != null)
                {
                    field.Name = "batch";
                    field.Type = TType.Struct;
                    field.ID = 1;
                    await oprot.WriteFieldBeginAsync(field, cancellationToken);
                    await Batch.WriteAsync(oprot, cancellationToken);
                    await oprot.WriteFieldEndAsync(cancellationToken);
                }
                await oprot.WriteFieldStopAsync(cancellationToken);
                await oprot.WriteStructEndAsync(cancellationToken);
            }
            finally
            {
                oprot.DecrementRecursionDepth();
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder("emitBatch_args(");
            if (Batch != null)
            {
                sb.Append("Batch: ");
                sb.Append(Batch == null ? "<null>" : Batch.ToString());
            }
            sb.Append(")");
            return sb.ToString();
        }
    }
}
