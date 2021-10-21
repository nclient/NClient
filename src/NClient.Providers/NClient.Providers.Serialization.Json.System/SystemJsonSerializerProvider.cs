using System.Text.Json;
using NClient.Abstractions.Providers.Serialization;
using NClient.Common.Helpers;

namespace NClient.Providers.Serialization.Json.System
{
    /// <summary>
    /// The System.Text.Json based provider for a component that can create <see cref="ISerializer"/> instances.
    /// </summary>
    public class SystemJsonSerializerProvider : ISerializerProvider
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        /// <summary>
        /// Creates the System.Text.Json based serializer provider.
        /// </summary>
        public SystemJsonSerializerProvider()
        {
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        /// <summary>
        /// Creates the System.Text.Json based serializer provider.
        /// </summary>
        /// <param name="jsonSerializerOptions">The options to be used with <see cref="JsonSerializer"/>.</param>
        public SystemJsonSerializerProvider(JsonSerializerOptions jsonSerializerOptions)
        {
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public ISerializer Create()
        {
            return new SystemJsonSerializer(_jsonSerializerOptions);
        }
    }
}
