namespace OpenCensus.Exporter.Jaeger
{
    using System;
    using OpenCensus.Exporter.Jaeger.Implimentation;
    using OpenCensus.Trace.Export;

    public class JaegerExporter : IDisposable
    {
        private const string ExporterName = "JaegerTraceExporter";

        private readonly object lck = new object();
        private readonly JaegerExporterOptions options;
        private readonly IExportComponent exportComponent;

        private bool isInitialized = false;
        private JaegerTraceExporterHandler handler;

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
                    this.handler = new JaegerTraceExporterHandler(this.options);
                    this.exportComponent.SpanExporter.RegisterHandler(ExporterName, this.handler);
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.handler.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~JaegerExporter()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
