
namespace OpenCensus.Exporter.Jaeger.Implimentation
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Thrift.Protocols;
    using Thrift.Protocols.Entities;
    using Thrift.Protocols.Utilities;

    public partial class Batch : TAbstractBase
    {
        public Batch()
        {
        }

        public Batch(Process process, List<JaegerSpan> spans) : this()
        {
            this.Process = process;
            this.Spans = spans;
        }

        public Process Process { get; set; }

        public List<JaegerSpan> Spans { get; set; }


        public async Task WriteAsync(TProtocol oprot, CancellationToken cancellationToken)
        {
            oprot.IncrementRecursionDepth();
            try
            {
                var struc = new TStruct("Batch");
                await oprot.WriteStructBeginAsync(struc, cancellationToken);
                var field = new TField();
                field.Name = "process";
                field.Type = TType.Struct;
                field.ID = 1;
                await oprot.WriteFieldBeginAsync(field, cancellationToken);
                await Process.WriteAsync(oprot, cancellationToken);
                await oprot.WriteFieldEndAsync(cancellationToken);
                field.Name = "spans";
                field.Type = TType.List;
                field.ID = 2;
                await oprot.WriteFieldBeginAsync(field, cancellationToken);
                {
                    await oprot.WriteListBeginAsync(new TList(TType.Struct, Spans.Count), cancellationToken);
                    foreach (JaegerSpan s in Spans)
                    {
                        await s.WriteAsync(oprot, cancellationToken);
                    }
                    await oprot.WriteListEndAsync(cancellationToken);
                }
                await oprot.WriteFieldEndAsync(cancellationToken);
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
            var sb = new StringBuilder("Batch(");
            sb.Append(", Process: ");
            sb.Append(Process == null ? "<null>" : Process.ToString());
            sb.Append(", Spans: ");
            sb.Append(Spans);
            sb.Append(")");
            return sb.ToString();
        }
    }

}
