using NClient.Annotations.Abstractions;

namespace NClient.Annotations.Parameters
{
    /// <summary>
    /// Specifies that a parameter should be bound using route-data from the current request.
    /// </summary>
    public class RouteParamAttribute : ParamAttribute, INameProviderAttribute
    {
        /// <summary>
        /// Model name.
        /// </summary>
        public string? Name { get; set; }
    }
}
