namespace OpenCensus.Exporter.Jaeger.Implimentation
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Thrift.Protocols;
    using Thrift.Protocols.Entities;

    public partial class Process : TAbstractBase
    {
        public Process()
        {
        }

        public Process(string serviceName) : this()
        {
            this.ServiceName = serviceName;
        }

        public string ServiceName { get; set; }

        public List<JaegerTag> Tags { get; set; }


        public async Task WriteAsync(TProtocol oprot, CancellationToken cancellationToken)
        {
            oprot.IncrementRecursionDepth();
            try
            {
                var struc = new TStruct("Process");
                await oprot.WriteStructBeginAsync(struc, cancellationToken);
                var field = new TField();
                field.Name = "serviceName";
                field.Type = TType.String;
                field.ID = 1;
                await oprot.WriteFieldBeginAsync(field, cancellationToken);
                await oprot.WriteStringAsync(ServiceName, cancellationToken);
                await oprot.WriteFieldEndAsync(cancellationToken);
                if (Tags != null)
                {
                    field.Name = "tags";
                    field.Type = TType.List;
                    field.ID = 2;
                    await oprot.WriteFieldBeginAsync(field, cancellationToken);
                    {
                        await oprot.WriteListBeginAsync(new TList(TType.Struct, Tags.Count), cancellationToken);
                        foreach (JaegerTag jt in Tags)
                        {
                            await jt.WriteAsync(oprot, cancellationToken);
                        }
                        await oprot.WriteListEndAsync(cancellationToken);
                    }
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
            var sb = new StringBuilder("Process(");
            sb.Append(", ServiceName: ");
            sb.Append(ServiceName);
            if (Tags != null)
            {
                sb.Append(", Tags: ");
                sb.Append(Tags);
            }
            sb.Append(")");
            return sb.ToString();
        }
    }
}
