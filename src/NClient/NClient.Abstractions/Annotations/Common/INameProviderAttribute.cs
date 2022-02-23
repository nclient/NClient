// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Interface for attributes which can supply a name for attribute.</summary>
    public interface INameProviderAttribute
    {
        /// <summary>Gets or sets the route name. The route name can be used to generate a link using a specific route, instead
        /// of relying on selection of a route based on the given set of route values.</summary>
        string? Name { get; }
    }
}
