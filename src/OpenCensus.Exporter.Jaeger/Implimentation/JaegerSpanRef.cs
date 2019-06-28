using System;
using Thrift.Protocols;

namespace OpenCensus.Exporter.Jaeger.Implimentation
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Thrift.Protocols;
    using Thrift.Protocols.Entities;

    public class JaegerSpanRef : TAbstractBase
    {
        public JaegerSpanRef()
        {
        }

        public JaegerSpanRef(JaegerSpanRefType refType, long traceIdLow, long traceIdHigh, long spanId) : this()
        {
            this.RefType = refType;
            this.TraceIdLow = traceIdLow;
            this.TraceIdHigh = traceIdHigh;
            this.SpanId = spanId;
        }

        public JaegerSpanRefType RefType { get; set; }

        public long TraceIdLow { get; set; }

        public long TraceIdHigh { get; set; }

        public long SpanId { get; set; }


        public async Task WriteAsync(TProtocol oprot, CancellationToken cancellationToken)
        {
            oprot.IncrementRecursionDepth();
            try
            {
                var struc = new TStruct("SpanRef");
                await oprot.WriteStructBeginAsync(struc, cancellationToken);
                var field = new TField();
                field.Name = "refType";
                field.Type = TType.I32;
                field.ID = 1;
                await oprot.WriteFieldBeginAsync(field, cancellationToken);
                await oprot.WriteI32Async((int)RefType, cancellationToken);
                await oprot.WriteFieldEndAsync(cancellationToken);
                field.Name = "traceIdLow";
                field.Type = TType.I64;
                field.ID = 2;
                await oprot.WriteFieldBeginAsync(field, cancellationToken);
                await oprot.WriteI64Async(TraceIdLow, cancellationToken);
                await oprot.WriteFieldEndAsync(cancellationToken);
                field.Name = "traceIdHigh";
                field.Type = TType.I64;
                field.ID = 3;
                await oprot.WriteFieldBeginAsync(field, cancellationToken);
                await oprot.WriteI64Async(TraceIdHigh, cancellationToken);
                await oprot.WriteFieldEndAsync(cancellationToken);
                field.Name = "spanId";
                field.Type = TType.I64;
                field.ID = 4;
                await oprot.WriteFieldBeginAsync(field, cancellationToken);
                await oprot.WriteI64Async(SpanId, cancellationToken);
                await oprot.WriteFieldEndAsync(cancellationToken);
                await oprot.WriteFieldStopAsync(cancellationToken);
                await oprot.WriteStructEndAsync(cancellationToken);
            }
            finally
            {
                oprot.DecrementRecursionDepth();
            }
        }

        /// <summary>
        /// 
        /// <seealso cref="SpanRefType"/>
        /// </summary>

        public override string ToString()
        {
            var sb = new StringBuilder("SpanRef(");
            sb.Append(", RefType: ");
            sb.Append(RefType);
            sb.Append(", TraceIdLow: ");
            sb.Append(TraceIdLow);
            sb.Append(", TraceIdHigh: ");
            sb.Append(TraceIdHigh);
            sb.Append(", SpanId: ");
            sb.Append(SpanId);
            sb.Append(")");
            return sb.ToString();
        }
    }
}
