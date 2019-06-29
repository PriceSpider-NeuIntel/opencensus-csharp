using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Thrift.Transports;

namespace OpenCensus.Exporter.Jaeger.Implimentation
{
    internal class InMemoryTransport : TClientTransport
    {
        private readonly MemoryStream _byteStream;
        private bool _isDisposed;

        public InMemoryTransport()
        {
            _byteStream = new MemoryStream();
        }

        public override bool IsOpen => true;

        public override async Task OpenAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await Task.FromCanceled(cancellationToken);
            }
        }

        public override void Close()
        {
            // do nothing
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int length,
            CancellationToken cancellationToken)
        {
            return await _byteStream.ReadAsync(buffer, offset, length, cancellationToken);
        }

        public override async Task WriteAsync(byte[] buffer, CancellationToken cancellationToken)
        {
            await _byteStream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
        }

        public override async Task WriteAsync(byte[] buffer, int offset, int length, CancellationToken cancellationToken)
        {
            await _byteStream.WriteAsync(buffer, offset, length, cancellationToken);
        }

        public override async Task FlushAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await Task.FromCanceled(cancellationToken);
            }
        }

        public byte[] GetBuffer()
        {
            return _byteStream.ToArray();
        }

        public void Reset()
        {
            _byteStream.SetLength(0);
        }

        // IDisposable
        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _byteStream?.Dispose();
                }
            }
            _isDisposed = true;
        }
    }

}
