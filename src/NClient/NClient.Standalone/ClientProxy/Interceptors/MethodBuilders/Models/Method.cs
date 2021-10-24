using System;
using NClient.Annotations;
using NClient.Annotations.Operations;
using NClient.Annotations.Versioning;

namespace NClient.Standalone.ClientProxy.Interceptors.MethodBuilders.Models
{
    // TODO: doc
    public class Method
    {
        public string Name { get; }
        public string ClientName { get; }
        public OperationAttribute Operation { get; }
        public PathAttribute? PathAttribute { get; set; }
        public UseVersionAttribute? UseVersionAttribute { get; set; }
        public HeaderAttribute[] HeaderAttributes { get; set; }
        public MethodParam[] Params { get; }

        public Method(string name, string clientName, OperationAttribute operation, MethodParam[] @params)
        {
            Name = name;
            ClientName = clientName;
            Operation = operation;
            Params = @params;
            HeaderAttributes = Array.Empty<HeaderAttribute>();
        }
    }
}
