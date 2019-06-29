using System;

namespace OpenCensus.Exporter.Jaeger
{
    public class JaegerExporterOptions
    {
        public string ServiceName { get; set; }
        public string AgentHost { get; set; }
        public int? AgentPort { get; set; }
        public int? MaxPacketSize { get; set; }
    }
}
