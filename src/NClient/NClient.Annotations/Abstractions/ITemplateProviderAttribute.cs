// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    public interface ITemplateProviderAttribute
    {
        /// <summary>
        /// The route template. May be null.
        /// </summary>
        string? Template { get; }
    }
}
