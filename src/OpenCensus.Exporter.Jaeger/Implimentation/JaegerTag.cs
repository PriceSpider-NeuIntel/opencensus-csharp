namespace OpenCensus.Exporter.Jaeger.Implimentation
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Thrift.Protocols;
    using Thrift.Protocols.Entities;

    public partial class JaegerTag : TAbstractBase
    {
        public string Key { get; set; }

        /// <summary>
        /// 
        /// <seealso cref="TagType"/>
        /// </summary>


        public JaegerTag()
        {
        }

        public JaegerTag(string key, JaegerTagType vType) : this()
        {
            this.Key = key;
            this.VType = vType;
        }

        public JaegerTagType VType { get; set; }

        public string VStr { get; set; }

        public double? VDouble { get; set; }

        public bool? VBool { get; set; }
        
        public long? VLong { get; set; }

        public byte[] VBinary {get; set; }

        public async Task WriteAsync(TProtocol oprot, CancellationToken cancellationToken)
        {
            oprot.IncrementRecursionDepth();
            try
            {
                var struc = new TStruct("Tag");
                await oprot.WriteStructBeginAsync(struc, cancellationToken);
                var field = new TField();
                field.Name = "key";
                field.Type = TType.String;
                field.ID = 1;
                await oprot.WriteFieldBeginAsync(field, cancellationToken);
                await oprot.WriteStringAsync(this.Key, cancellationToken);
                await oprot.WriteFieldEndAsync(cancellationToken);
                field.Name = "vType";
                field.Type = TType.I32;
                field.ID = 2;
                await oprot.WriteFieldBeginAsync(field, cancellationToken);
                await oprot.WriteI32Async((int)this.VType, cancellationToken);
                await oprot.WriteFieldEndAsync(cancellationToken);
                if (VStr != null)
                {
                    field.Name = "vStr";
                    field.Type = TType.String;
                    field.ID = 3;
                    await oprot.WriteFieldBeginAsync(field, cancellationToken);
                    await oprot.WriteStringAsync(this.VStr, cancellationToken);
                    await oprot.WriteFieldEndAsync(cancellationToken);
                }
                if (VDouble.HasValue)
                {
                    field.Name = "vDouble";
                    field.Type = TType.Double;
                    field.ID = 4;
                    await oprot.WriteFieldBeginAsync(field, cancellationToken);
                    await oprot.WriteDoubleAsync(VDouble.Value, cancellationToken);
                    await oprot.WriteFieldEndAsync(cancellationToken);
                }
                if (VBool.HasValue)
                {
                    field.Name = "vBool";
                    field.Type = TType.Bool;
                    field.ID = 5;
                    await oprot.WriteFieldBeginAsync(field, cancellationToken);
                    await oprot.WriteBoolAsync(VBool.Value, cancellationToken);
                    await oprot.WriteFieldEndAsync(cancellationToken);
                }
                if (VLong.HasValue)
                {
                    field.Name = "vLong";
                    field.Type = TType.I64;
                    field.ID = 6;
                    await oprot.WriteFieldBeginAsync(field, cancellationToken);
                    await oprot.WriteI64Async(VLong.Value, cancellationToken);
                    await oprot.WriteFieldEndAsync(cancellationToken);
                }
                if (VBinary != null)
                {
                    field.Name = "vBinary";
                    field.Type = TType.String;
                    field.ID = 7;
                    await oprot.WriteFieldBeginAsync(field, cancellationToken);
                    await oprot.WriteBinaryAsync(VBinary, cancellationToken);
                    await oprot.WriteFieldEndAsync(cancellationToken);
                }
                await oprot.WriteFieldStopAsync(cancellationToken);
                await oprot.WriteStructEndAsync(cancellationToken);
            }
            finally
            {
                oprot.DecrementRecursionDepth();
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder("Tag(");
            sb.Append(", Key: ");
            sb.Append(Key);
            sb.Append(", VType: ");
            sb.Append(VType);
            if (VStr != null)
            {
                sb.Append(", VStr: ");
                sb.Append(VStr);
            }
            if (VDouble.HasValue)
            {
                sb.Append(", VDouble: ");
                sb.Append(VDouble);
            }
            if (VBool.HasValue)
            {
                sb.Append(", VBool: ");
                sb.Append(VBool);
            }
            if (VLong.HasValue)
            {
                sb.Append(", VLong: ");
                sb.Append(VLong);
            }
            if (VBinary != null)
            {
                sb.Append(", VBinary: ");
                sb.Append(VBinary);
            }
            sb.Append(")");
            return sb.ToString();
        }
    }

}
