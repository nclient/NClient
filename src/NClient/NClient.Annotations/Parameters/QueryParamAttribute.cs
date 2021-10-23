using System;

namespace NClient.Annotations.Parameters
{
    /// <summary>
    /// Specifies that a parameter should be bound using the request query string.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class QueryParamAttribute : ParamAttribute, INameProviderAttribute
    {
        /// <summary>
        /// Model name.
        /// </summary>
        public string? Name { get; set; }
    }
}
