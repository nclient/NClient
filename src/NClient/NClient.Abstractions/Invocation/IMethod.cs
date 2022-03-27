using System;
using System.Reflection;
using NClient.Annotations;

namespace NClient.Invocation
{
    /// <summary>Information about the client's executable method.</summary>
    public interface IMethod
    {
        /// <summary>Get method name.</summary>
        string Name { get; }
        
        /// <summary>Gets information about the client method.</summary>
        MethodInfo Info { get; }
        
        /// <summary>Get client name.</summary>
        string ClientName { get; }
        
        /// <summary>Gets type of client interface.</summary>
        Type ClientType { get; }
        
        /// <summary>Get type of operation.</summary>
        IOperationAttribute Operation { get; }
        
        /// <summary>Get <see cref="IPathAttribute"/> represents part of URI for method.</summary>
        IPathAttribute? PathAttribute { get; }
        
        /// <summary>Get <see cref="IUseVersionAttribute"/>.</summary>
        IUseVersionAttribute? UseVersionAttribute { get; }
        
        /// <summary>Get <see cref="ITimeoutAttribute"/> represents time limit for method execution.</summary>
        ITimeoutAttribute? TimeoutAttribute { get; }
        
        /// <summary>Get <see cref="ICachingAttribute"/> represents time limit for cahing.</summary>
        ICachingAttribute? CachingAttribute { get; }
        
        /// <summary>Get array of <see cref="IMetadataAttribute"/> represents additional method info (like headers for HTTP).</summary>
        IMetadataAttribute[] MetadataAttributes { get; }
        
        /// <summary>Get array of <see cref="IMethodParam"/> represents incoming parameters of operation.</summary>
        IMethodParam[] Params { get; }
        
        /// <summary>Gets type returned by the client method.</summary>
        Type ResultType { get; }
    }
}
