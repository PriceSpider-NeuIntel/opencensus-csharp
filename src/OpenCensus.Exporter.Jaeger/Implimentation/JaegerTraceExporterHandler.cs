namespace OpenCensus.Exporter.Jaeger.Implimentation
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using OpenCensus.Trace.Export;

    public class JaegerTraceExporterHandler : IHandler
    {
        private readonly UdpClient udpClient;

        public JaegerTraceExporterHandler(JaegerExporterOptions options)
        {
            this.udpClient = new UdpClient(options.AgentHost, options.AgentPort);
        }

        public Task ExportAsync(IEnumerable<ISpanData> spanDataList)
        {

            throw new NotImplementedException();
        }
    }
}
