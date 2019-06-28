using System;

namespace OpenCensus.Exporter.Jaeger.Implimentation
{
    public enum JaegerSpanRefType
    {
        CHILD_OF = 0,
        FOLLOWS_FROM = 1,
    }
}
