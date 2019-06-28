
namespace OpenCensus.Exporter.Jaeger.Implimentation
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Thrift.Protocols;
    using Thrift.Protocols.Entities;

    public partial class JaegerSpan : TAbstractBase
    {
        public JaegerSpan()
        {
        }

        public JaegerSpan(long traceIdLow, long traceIdHigh, long spanId, long parentSpanId, string operationName, int flags, long startTime, long duration) : this()
        {
            this.TraceIdLow = traceIdLow;
            this.TraceIdHigh = traceIdHigh;
            this.SpanId = spanId;
            this.ParentSpanId = parentSpanId;
            this.OperationName = operationName;
            this.Flags = flags;
            this.StartTime = startTime;
            this.Duration = duration;
        }

        public long TraceIdLow { get; set; }

        public long TraceIdHigh { get; set; }

        public long SpanId { get; set; }

        public long ParentSpanId { get; set; }

        public string OperationName { get; set; }

        public List<JaegerSpanRef> References { get; set; }

        public int Flags { get; set; }

        public long StartTime { get; set; }

        public long Duration { get; set; }

        public List<JaegerTag> JaegerTags { get; set; }

        public List<JaegerLog> Logs { get; set; }

        public async Task WriteAsync(TProtocol oprot, CancellationToken cancellationToken)
        {
            oprot.IncrementRecursionDepth();
            try
            {
                var struc = new TStruct("Span");
                await oprot.WriteStructBeginAsync(struc, cancellationToken);
                var field = new TField();
                field.Name = "traceIdLow";
                field.Type = TType.I64;
                field.ID = 1;
                await oprot.WriteFieldBeginAsync(field, cancellationToken);
                await oprot.WriteI64Async(this.TraceIdLow, cancellationToken);
                await oprot.WriteFieldEndAsync(cancellationToken);
                field.Name = "traceIdHigh";
                field.Type = TType.I64;
                field.ID = 2;
                await oprot.WriteFieldBeginAsync(field, cancellationToken);
                await oprot.WriteI64Async(this.TraceIdHigh, cancellationToken);
                await oprot.WriteFieldEndAsync(cancellationToken);
                field.Name = "spanId";
                field.Type = TType.I64;
                field.ID = 3;
                await oprot.WriteFieldBeginAsync(field, cancellationToken);
                await oprot.WriteI64Async(this.SpanId, cancellationToken);
                await oprot.WriteFieldEndAsync(cancellationToken);
                field.Name = "parentSpanId";
                field.Type = TType.I64;
                field.ID = 4;
                await oprot.WriteFieldBeginAsync(field, cancellationToken);
                await oprot.WriteI64Async(this.ParentSpanId, cancellationToken);
                await oprot.WriteFieldEndAsync(cancellationToken);
                field.Name = "operationName";
                field.Type = TType.String;
                field.ID = 5;
                await oprot.WriteFieldBeginAsync(field, cancellationToken);
                await oprot.WriteStringAsync(this.OperationName, cancellationToken);
                await oprot.WriteFieldEndAsync(cancellationToken);
                if (this.References != null)
                {
                    field.Name = "references";
                    field.Type = TType.List;
                    field.ID = 6;
                    await oprot.WriteFieldBeginAsync(field, cancellationToken);
                    {
                        await oprot.WriteListBeginAsync(new TList(TType.Struct, References.Count), cancellationToken);
                        foreach (JaegerSpanRef sr in References)
                        {
                            await sr.WriteAsync(oprot, cancellationToken);
                        }
                        await oprot.WriteListEndAsync(cancellationToken);
                    }
                    await oprot.WriteFieldEndAsync(cancellationToken);
                }
                field.Name = "flags";
                field.Type = TType.I32;
                field.ID = 7;
                await oprot.WriteFieldBeginAsync(field, cancellationToken);
                await oprot.WriteI32Async(this.Flags, cancellationToken);
                await oprot.WriteFieldEndAsync(cancellationToken);
                field.Name = "startTime";
                field.Type = TType.I64;
                field.ID = 8;
                await oprot.WriteFieldBeginAsync(field, cancellationToken);
                await oprot.WriteI64Async(this.StartTime, cancellationToken);
                await oprot.WriteFieldEndAsync(cancellationToken);
                field.Name = "duration";
                field.Type = TType.I64;
                field.ID = 9;
                await oprot.WriteFieldBeginAsync(field, cancellationToken);
                await oprot.WriteI64Async(this.Duration, cancellationToken);
                await oprot.WriteFieldEndAsync(cancellationToken);
                if (this.JaegerTags != null)
                {
                    field.Name = "JaegerTags";
                    field.Type = TType.List;
                    field.ID = 10;
                    await oprot.WriteFieldBeginAsync(field, cancellationToken);
                    {
                        await oprot.WriteListBeginAsync(new TList(TType.Struct, JaegerTags.Count), cancellationToken);
                        foreach (JaegerTag jt in this.JaegerTags)
                        {
                            await jt.WriteAsync(oprot, cancellationToken);
                        }
                        await oprot.WriteListEndAsync(cancellationToken);
                    }
                    await oprot.WriteFieldEndAsync(cancellationToken);
                }
                if (this.Logs != null)
                {
                    field.Name = "logs";
                    field.Type = TType.List;
                    field.ID = 11;
                    await oprot.WriteFieldBeginAsync(field, cancellationToken);
                    {
                        await oprot.WriteListBeginAsync(new TList(TType.Struct, Logs.Count), cancellationToken);
                        foreach (JaegerLog jl in this.Logs)
                        {
                            await jl.WriteAsync(oprot, cancellationToken);
                        }
                        await oprot.WriteListEndAsync(cancellationToken);
                    }
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
            var sb = new StringBuilder("Span(");
            sb.Append(", TraceIdLow: ");
            sb.Append(TraceIdLow);
            sb.Append(", TraceIdHigh: ");
            sb.Append(TraceIdHigh);
            sb.Append(", SpanId: ");
            sb.Append(SpanId);
            sb.Append(", ParentSpanId: ");
            sb.Append(ParentSpanId);
            sb.Append(", OperationName: ");
            sb.Append(OperationName);
            if (References != null)
            {
                sb.Append(", References: ");
                sb.Append(References);
            }
            sb.Append(", Flags: ");
            sb.Append(Flags);
            sb.Append(", StartTime: ");
            sb.Append(StartTime);
            sb.Append(", Duration: ");
            sb.Append(Duration);
            if (JaegerTags != null)
            {
                sb.Append(", JaegerTags: ");
                sb.Append(JaegerTags);
            }
            if (Logs != null)
            {
                sb.Append(", Logs: ");
                sb.Append(Logs);
            }
            sb.Append(")");
            return sb.ToString();
        }
    }

}
