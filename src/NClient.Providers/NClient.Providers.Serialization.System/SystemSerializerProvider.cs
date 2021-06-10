using System.Text.Json;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;

namespace NClient.Providers.Serialization.System
{
    /// <summary>
    /// The System.Text.Json based provider for a component that can create <see cref="ISerializer"/> instances.
    /// </summary>
    public class SystemSerializerProvider : ISerializerProvider
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        /// <summary>
        /// Creates the System.Text.Json based serializer provider.
        /// </summary>
        public SystemSerializerProvider()
        {
            _jsonSerializerOptions = new JsonSerializerOptions();
        }

        /// <summary>
        /// Creates the System.Text.Json based serializer provider.
        /// </summary>
        /// <param name="jsonSerializerOptions">The options to be used with <see cref="JsonSerializer"/>.</param>
        public SystemSerializerProvider(JsonSerializerOptions jsonSerializerOptions)
        {
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public ISerializer Create()
        {
            return new SystemSerializer(_jsonSerializerOptions);
        }
    }
}