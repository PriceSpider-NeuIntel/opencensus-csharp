
namespace OpenCensus.Exporter.Jaeger.Implimentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OpenCensus.Common;
    using OpenCensus.Trace;
    using OpenCensus.Trace.Export;

    public static class JaegerConversionExtensions
    {
        private const long MillisPerSecond = 1000L;
        private const long NanosPerMillisecond = 1000 * 1000;
        private const long NanosPerSecond = NanosPerMillisecond * MillisPerSecond;

        public static JaegerSpan ToJaegerSpan(this ISpanData span)
        {
            var jaegerTags = new List<JaegerTag>();

            if (span?.Attributes?.AttributeMap != null)
            {
                jaegerTags.AddRange(span.Attributes.AttributeMap.Select(a => a.ToJaegerTag()));
            }

            var jaegerLogs = new List<JaegerLog>();

            if (span?.MessageEvents?.Events != null)
            {
                jaegerLogs.AddRange(span.MessageEvents.Events.Select(e => e.ToJaegerLog()));
            }

            if (span?.Annotations?.Events != null)
            {
                jaegerLogs.AddRange(span.Annotations.Events.Select(e => e.ToJaegerLog()));
            }

            var refs = new List<JaegerSpanRef>();

            if (span?.Links?.Links != null)
            {
                refs.AddRange(span.Links.Links.Select(l => l.ToJaegerSpanRef()).Where(l => l != null));
            }

            Int128? parentSpanId = null;

            if (span?.ParentSpanId?.Bytes != null)
            {
                parentSpanId = new Int128(span.ParentSpanId.Bytes);
            }

            var traceId = span?.Context?.TraceId?.Bytes == null ? Int128.Empty : new Int128(span.Context.TraceId.Bytes);
            var spanId = span?.Context?.SpanId?.Bytes == null ? Int128.Empty : new Int128(span.Context.SpanId.Bytes);

            return new JaegerSpan
            {
                TraceIdHigh = traceId.High,
                TraceIdLow = traceId.Low,
                SpanId = spanId.Low,
                ParentSpanId = parentSpanId?.Low ?? 0,
                OperationName = span.Name,
                References = refs.Count == 0 ? null : refs,
                Flags = span.Context.TraceOptions.IsSampled ? 0x1 : 0,
                StartTime = ToEpochMicroseconds(span.StartTimestamp),
                Duration = ToEpochMicroseconds(span.EndTimestamp) - ToEpochMicroseconds(span.StartTimestamp),
                JaegerTags = jaegerTags,
                Logs = jaegerLogs
            };
        }

        public static JaegerTag ToJaegerTag(this KeyValuePair<string, IAttributeValue> attribute)
        {
            var ret = attribute.Value.Match(
                (s) => new JaegerTag { Key = attribute.Key, VType = JaegerTagType.STRING, VStr = s },
                (b) => new JaegerTag { Key = attribute.Key, VType = JaegerTagType.BOOL, VBool = b },
                (l) => new JaegerTag { Key = attribute.Key, VType = JaegerTagType.LONG, VLong = l },
                (d) => new JaegerTag { Key = attribute.Key, VType = JaegerTagType.DOUBLE, VDouble = d },
                (obj) => new JaegerTag { Key = attribute.Key, VType = JaegerTagType.STRING, VStr = obj.ToString() });

            return ret;
        }

        public static JaegerLog ToJaegerLog(this ITimedEvent<IMessageEvent> messageEvent)
        {

            return new JaegerLog
            {
                Timestamp = messageEvent.Timestamp.ToEpochMicroseconds(),
                Fields = new List<JaegerTag> {
                    new JaegerTag { Key = "message.id", VType = JaegerTagType.LONG, VLong = messageEvent.Event.MessageId },
                    new JaegerTag { Key = "message.type", VType = JaegerTagType.LONG, VLong = (long)messageEvent.Event.Type }
                }
            };
        }

        public static JaegerLog ToJaegerLog(this ITimedEvent<IAnnotation> annotation)
        {
            var tags = annotation.Event.Attributes.Select(a => a.ToJaegerTag()).ToList();
            tags.Add(new JaegerTag { Key = "description", VType = JaegerTagType.STRING, VStr = annotation.Event.Description });

            return new JaegerLog
            {
                Timestamp = annotation.Timestamp.ToEpochMicroseconds(),
                Fields = tags
            };
        }

        public static JaegerSpanRef ToJaegerSpanRef(this ILink link)
        {
            var spanRefType = ConvertLinkType(link.Type);

            if (spanRefType == null)
            {
                return null;
            }

            var traceId = link?.TraceId.Bytes == null ? Int128.Empty : new Int128(link.TraceId.Bytes);
            var spanId = link?.SpanId?.Bytes == null ? Int128.Empty : new Int128(link.SpanId.Bytes);

            return new JaegerSpanRef
            {
                TraceIdHigh = traceId.High,
                TraceIdLow = traceId.Low,
                SpanId = spanId.Low,
                RefType = spanRefType.Value
            };
        }

        private static JaegerSpanRefType? ConvertLinkType(LinkType linkType)
        {
            if (linkType == LinkType.ChildLinkedSpan)
            {
                return JaegerSpanRefType.CHILD_OF;
            }

            if (linkType == LinkType.ParentLinkedSpan)
            {
                return JaegerSpanRefType.FOLLOWS_FROM;
            }

            return null;
        }

        private static long ToEpochMicroseconds(this Timestamp timestamp)
        {
            long nanos = (timestamp.Seconds * NanosPerSecond) + timestamp.Nanos;
            long micros = nanos / 1000L;
            return micros;
        }
    }
}
