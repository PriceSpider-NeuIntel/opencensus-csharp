namespace OpenCensus.Exporter.Jaeger
{
    using System;
    using OpenCensus.Exporter.Jaeger.Implimentation;
    using OpenCensus.Trace.Export;

    public class JaegerExporter
    {
        private const string ExporterName = "JaegerTraceExporter";

        private readonly object lck = new object();
        private readonly JaegerExporterOptions options;
        private readonly IExportComponent exportComponent;

        private bool isInitialized = false;

        public JaegerExporter(JaegerExporterOptions options, IExportComponent exportComponent)
        {
            this.options = options;
            this.exportComponent = exportComponent;
        }

        public void Start()
        {
            lock (this.lck)
            {
                if (this.isInitialized)
                {
                    return;
                }

                if (this.exportComponent != null)
                {
                    this.exportComponent.SpanExporter.RegisterHandler(ExporterName, new JaegerTraceExporterHandler(this.options));
                }
            }
        }

        public void Stop()
        {
            if (!this.isInitialized)
            {
                return;
            }

            lock (this.lck)
            {
                if (this.exportComponent != null)
                {
                    this.exportComponent.SpanExporter.UnregisterHandler(ExporterName);
                }
            }

            this.isInitialized = false;
        }
    }
}
