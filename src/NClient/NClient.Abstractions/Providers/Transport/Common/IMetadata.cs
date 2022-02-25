// ReSharper disable once CheckNamespace

namespace NClient.Providers.Transport
{
    /// <summary>Represents additional information of a request or response.</summary>
    public interface IMetadata
    {
        /// <summary>Gets metadata name.</summary>
        string Name { get; }
        
        /// <summary>Gets metadata value.</summary>
        string Value { get; }
    }
}
