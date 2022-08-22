// ReSharper disable once CheckNamespace

namespace NClient.Annotations.Http
{
    /// <summary>Specifies that a parameter should be bound using route-data from the current request.</summary>
    public class RouteParamAttribute : PathParamAttribute, IRouteParamAttribute
    {
        /// <summary>Initializes a new <see cref="RouteParamAttribute"/>.</summary>
        public RouteParamAttribute()
        {
        }
        
        /// <summary>Initializes a new <see cref="RouteParamAttribute"/> with the given route parameter.</summary>
        /// <param name="name">The route parameter name.</param>
        public RouteParamAttribute(string name) : base(name)
        {
        }
    }
}
