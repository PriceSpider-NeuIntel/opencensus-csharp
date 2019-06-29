namespace OpenCensus.Exporter.Jaeger.Implimentation
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;
    using Thrift.Transports;

    public class JaegerThriftClientTransport : TClientTransport
    {
        private readonly UdpClient udpClient;
        private readonly MemoryStream byteStream;
        private bool isDisposed = false;

        public JaegerThriftClientTransport(string host, int port)
        {
            this.byteStream = new MemoryStream();
            this.udpClient = new UdpClient();
            this.udpClient.Connect(host, port);
        }

        public override bool IsOpen => this.udpClient.Client.Connected;

        public override void Close()
        {
            this.udpClient.Close();
        }

        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            var bytes = this.byteStream.ToArray();

            if (bytes.Length == 0)
            {
                return Task.CompletedTask;
            }

            this.byteStream.SetLength(0);

            try
            {
                return this.udpClient.SendAsync(bytes, bytes.Length);
            }
            catch (SocketException se)
            {
                throw new TTransportException(TTransportException.ExceptionType.Unknown, $"Cannot flush because of socket exception. UDP Packet size was {bytes.Length}. Exception message: {se.Message}");
            }
            catch (Exception e)
            {
                throw new TTransportException(TTransportException.ExceptionType.Unknown, $"Cannot flush closed transport. {e.Message}");
            }
        }

        public override Task OpenAsync(CancellationToken cancellationToken)
        {
            // Do nothing
            return Task.CompletedTask;
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int length, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task WriteAsync(byte[] buffer, CancellationToken cancellationToken)
        {
            return WriteAsync(buffer, 0, buffer.Length, cancellationToken);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int length, CancellationToken cancellationToken)
        {
            return this.byteStream.WriteAsync(buffer, offset, length, cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.isDisposed && disposing)
            {
                this.byteStream?.Dispose();
                this.udpClient?.Dispose();
            }
            this.isDisposed = true;
        }

        public override string ToString()
        {
            return $"{nameof(JaegerThriftClientTransport)}(Client={udpClient.Client.RemoteEndPoint})";
        }
    }
}
