using System;

namespace OpenCensus.Exporter.Jaeger.Implimentation
{
    public class JaegerExporterException : Exception
    {
        public JaegerExporterException(string message, Exception originalException)
            : base(message, originalException)
        {
        }
    }
}
