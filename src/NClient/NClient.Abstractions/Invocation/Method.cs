using System;
using System.Reflection;
using NClient.Annotations;

namespace NClient.Invocation
{
    internal class Method : IMethod
    {
        public string Name { get; }
        public MethodInfo Info { get; }
        public string ClientName { get; }
        public Type ClientType { get; }
        public IOperationAttribute Operation { get; }
        public IPathAttribute? PathAttribute { get; set; }
        public IUseVersionAttribute? UseVersionAttribute { get; set; }
        public IMetadataAttribute[] MetadataAttributes { get; set; }
        public ITimeoutAttribute? TimeoutAttribute { get; set; }
        public ICachingAttribute? CachingAttribute { get; set; }
        public IMethodParam[] Params { get; }
        public Type ResultType { get; }

        public Method(string name, MethodInfo info, string clientName, Type clientType,
            IOperationAttribute operation, IMethodParam[] @params, Type returnType)
        {
            Name = name;
            Info = info;
            ClientName = clientName;
            ClientType = clientType;
            Operation = operation;
            Params = @params;
            MetadataAttributes = Array.Empty<IMetadataAttribute>();
            ResultType = returnType;
        }
    }
}
