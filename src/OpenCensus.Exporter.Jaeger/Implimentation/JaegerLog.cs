namespace OpenCensus.Exporter.Jaeger.Implimentation
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Thrift.Protocols;
    using Thrift.Protocols.Entities;

    public class JaegerLog : TAbstractBase
    {
        public JaegerLog()
        {
        }

        public JaegerLog(long timestamp, List<JaegerTag> fields) : this()
        {
            this.Timestamp = timestamp;
            this.Fields = fields;
        }

        public long Timestamp { get; set; }

        public List<JaegerTag> Fields { get; set; }

        public async Task WriteAsync(TProtocol oprot, CancellationToken cancellationToken)
        {
            oprot.IncrementRecursionDepth();
            try
            {
                var struc = new TStruct("Log");
                await oprot.WriteStructBeginAsync(struc, cancellationToken);
                var field = new TField();
                field.Name = "timestamp";
                field.Type = TType.I64;
                field.ID = 1;
                await oprot.WriteFieldBeginAsync(field, cancellationToken);
                await oprot.WriteI64Async(Timestamp, cancellationToken);
                await oprot.WriteFieldEndAsync(cancellationToken);
                field.Name = "fields";
                field.Type = TType.List;
                field.ID = 2;
                await oprot.WriteFieldBeginAsync(field, cancellationToken);
                {
                    await oprot.WriteListBeginAsync(new TList(TType.Struct, Fields.Count), cancellationToken);
                    foreach (JaegerTag jt in Fields)
                    {
                        await jt.WriteAsync(oprot, cancellationToken);
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
            var sb = new StringBuilder("Log(");
            sb.Append(", Timestamp: ");
            sb.Append(Timestamp);
            sb.Append(", Fields: ");
            sb.Append(Fields);
            sb.Append(")");
            return sb.ToString();
        }
    }
}
