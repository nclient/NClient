using System; 

// ReSharper disable once CheckNamespace
namespace NClient.Annotations.Http
{
    /// <summary>
    /// Specifies that a parameter should be bound using the request query string.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class QueryParamAttribute : PropertyParamAttribute, IQueryParamAttribute
    {
    }
}
