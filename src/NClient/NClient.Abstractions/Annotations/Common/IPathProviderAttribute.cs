// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    public interface IPathProviderAttribute
    {
        /// <summary>
        /// The route template. May be null.
        /// </summary>
        string? Path { get; }
    }
}
