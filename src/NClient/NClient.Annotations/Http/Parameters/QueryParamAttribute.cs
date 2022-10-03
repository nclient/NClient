using System; 

// ReSharper disable once CheckNamespace
namespace NClient.Annotations.Http
{
    /// <summary>Specifies that a parameter should be bound using the request query string.</summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class QueryParamAttribute : PropertyParamAttribute, IQueryParamAttribute
    {
        /// <summary>Initializes a new <see cref="QueryParamAttribute"/>.</summary>
        public QueryParamAttribute()
        {
        }

        /// <summary>Initializes a new <see cref="QueryParamAttribute"/> with the given query parameter.</summary>
        /// <param name="name">The query parameter name.</param>
        public QueryParamAttribute(string name) : base(name)
        {
        }
    }
}
