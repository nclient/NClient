// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    public interface IPathProviderAttribute
    {
        /// <summary>Gets or sets a route template.</summary>
        string? Path { get; }
    }
}
