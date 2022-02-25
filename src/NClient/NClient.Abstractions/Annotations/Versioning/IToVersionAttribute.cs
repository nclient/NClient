// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Represents the metadata that describes the API version-specific implementation of a service.</summary>
    public interface IToVersionAttribute
    {
        /// <summary>Gets the API version defined by the attribute.</summary>
        string Version { get; }
    }
}
