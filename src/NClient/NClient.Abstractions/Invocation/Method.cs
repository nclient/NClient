using System;
using System.Reflection;
using NClient.Annotations;

namespace NClient.Invocation
{
    public class Method : IMethod
    {
        public string Name { get; }
        
        /// <summary>
        /// Gets information about the client method. 
        /// </summary>
        public MethodInfo Info { get; }
        
        public string ClientName { get; }
        
        /// <summary>
        /// Gets type of client interface.
        /// </summary>
        public Type ClientType { get; }

        public IOperationAttribute Operation { get; }
        
        public IPathAttribute? PathAttribute { get; set; }
        
        public IUseVersionAttribute? UseVersionAttribute { get; set; }
        
        public IMetadataAttribute[] MetadataAttributes { get; set; }
        
        public ITimeoutAttribute? TimeoutAttribute { get; set; }
        
        public IMethodParam[] Params { get; }
        
        /// <summary>
        /// Gets type returned by the client method.
        /// </summary>
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
