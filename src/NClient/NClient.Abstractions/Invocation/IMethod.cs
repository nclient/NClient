using System;
using System.Reflection;
using NClient.Annotations;

namespace NClient.Invocation
{
    // TODO: doc
    public interface IMethod
    {
        string Name { get; }
        /// <summary>
        /// Gets information about the client method. 
        /// </summary>
        MethodInfo Info { get; }
        string ClientName { get; }
        /// <summary>
        /// Gets type of client interface.
        /// </summary>
        Type ClientType { get; }
        IOperationAttribute Operation { get; }
        IPathAttribute? PathAttribute { get; }
        IUseVersionAttribute? UseVersionAttribute { get; }
        ITimeoutAttribute? TimeoutAttribute { get; }
        IMetadataAttribute[] MetadataAttributes { get; }
        IMethodParam[] Params { get; }
        /// <summary>
        /// Gets type returned by the client method.
        /// </summary>
        Type ResultType { get; }
    }
}
