using System.Text.Json;
using Microsoft.Extensions.Logging;
using NClient.Common.Helpers;

namespace NClient.Providers.Serialization.SystemTextJson
{
    /// <summary>The System.Text.Json based provider for a component that can create <see cref="ISerializer"/> instances.</summary>
    public class SystemTextJsonSerializerProvider : ISerializerProvider
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        /// <summary>Initializes the System.Text.Json based serializer provider.</summary>
        public SystemTextJsonSerializerProvider()
        {
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        /// <summary>Initializes the System.Text.Json based serializer provider.</summary>
        /// <param name="jsonSerializerOptions">The options to be used with <see cref="JsonSerializer"/>.</param>
        public SystemTextJsonSerializerProvider(JsonSerializerOptions jsonSerializerOptions)
        {
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            _jsonSerializerOptions = jsonSerializerOptions;
        }
        
        /// <summary>Creates System.Text.Json <see cref="ISerializer"/> instance.</summary>
        /// <param name="logger">Optional logger. If it is not passed, then logs will not be written.</param>
        public ISerializer Create(ILogger? logger)
        {
            return new SystemTextJsonSerializer(_jsonSerializerOptions);
        }
    }
}
