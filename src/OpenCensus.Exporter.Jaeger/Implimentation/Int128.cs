using System;

namespace OpenCensus.Exporter.Jaeger.Implimentation
{
    public struct Int128
    {
        public static Int128 Empty = new Int128();

        public Int128(Byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            if (bytes.Length != 8 && bytes.Length != 16)
            {
                throw new ArgumentOutOfRangeException("Number of bytes must be 8 or 16");
            }

            if (bytes.Length == 8)
            {
                this.High = 0;
                this.Low = BitConverter.ToInt64(bytes, 0);
            }
            else
            {
                this.High = BitConverter.ToInt64(bytes, 0);
                this.Low = BitConverter.ToInt64(bytes, 8);
            }
        }
        public long High { get; set; }
        public long Low { get; set; }
    }
}
