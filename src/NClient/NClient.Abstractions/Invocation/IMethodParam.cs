using System;
using NClient.Annotations;

namespace NClient.Invocation
{
    /// <summary>Information about the method parameter.</summary>
    public interface IMethodParam
    {
        /// <summary>Gets parameter name.</summary>
        string Name { get; }
        
        /// <summary>Gets parameter type.</summary>
        Type Type { get; }
        
        /// <summary>Gets parameter attribute.</summary>
        IParamAttribute Attribute { get; }
    }
}
