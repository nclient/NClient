// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    public interface IUseVersionAttribute
    {
        /// <summary>Gets the API version defined by the attribute.</summary>
        string Version { get; }
    }
}
