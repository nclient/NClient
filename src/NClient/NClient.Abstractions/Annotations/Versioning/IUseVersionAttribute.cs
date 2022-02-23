// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Represents the metadata that describes the API version associated used by the client.</summary>
    public interface IUseVersionAttribute
    {
        /// <summary>Gets the API version defined by the attribute.</summary>
        string Version { get; }
    }
}
