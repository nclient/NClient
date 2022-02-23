using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    /// <summary>Represents additional information of a request or response.</summary>
    public record Metadata : IMetadata
    {
        /// <summary>Gets metadata name.</summary>
        public string Name { get; }
        
        /// <summary>Gets metadata value.</summary>
        public string Value { get; }

        public Metadata(string name, string value)
        {
            Ensure.IsNotNullOrEmpty(name, nameof(name));
            Ensure.IsNotNullOrEmpty(value, nameof(value));

            Name = name;
            Value = value;
        }
    }
}
