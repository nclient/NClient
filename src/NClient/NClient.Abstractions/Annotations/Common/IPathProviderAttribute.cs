// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Interface for attributes which can supply a route template for attribute.</summary>
    public interface IPathProviderAttribute
    {
        /// <summary>Gets or sets a route template.</summary>
        string? Path { get; }
    }
}
