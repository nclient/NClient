using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    /// <summary>
    /// The metadata of the request or response.
    /// </summary>
    public record Metadata : IMetadata
    {
        public string Name { get; }
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
