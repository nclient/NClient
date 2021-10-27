using System;
using NClient.Annotations;
using NClient.Attributes;

namespace NClient.Invocation
{
    // TODO: doc
    public class Method
    {
        public string Name { get; }
        public string ClientName { get; }
        public IOperationAttribute Operation { get; }
        public IPathAttribute? PathAttribute { get; set; }
        public IUseVersionAttribute? UseVersionAttribute { get; set; }
        public IMetadataAttribute[] MetadataAttributes { get; set; }
        public MethodParam[] Params { get; }

        public Method(string name, string clientName, IOperationAttribute operation, MethodParam[] @params)
        {
            Name = name;
            ClientName = clientName;
            Operation = operation;
            Params = @params;
            MetadataAttributes = Array.Empty<IMetadataAttribute>();
        }
    }
}
