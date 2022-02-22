namespace NClient.Annotations
{
    public interface IPathAttribute
    {
        /// <summary>Gets the route name. The route name can be used to generate a link using a specific route, instead
        /// of relying on selection of a route based on the given set of route values.</summary>
        string? Name { get; }
        
        /// <summary>
        /// Gets the route order. The order determines the order of route execution. Routes with a lower
        /// order value are tried first. When a route doesn't specify a value, it gets a default value of 0.
        /// A null value for the Order property means that the user didn't specify an explicit order for the route.</summary>
        int Order { get; } 
        
        /// <summary>The route template. May be null.</summary>
        string Template { get; }
    }
}
