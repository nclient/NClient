using Microsoft.Extensions.Logging;
using NClient.Common.Helpers;
using Newtonsoft.Json;

namespace NClient.Providers.Serialization.NewtonsoftJson
{
    /// <summary>
    /// The Newtonsoft.Json based provider for a component that can create <see cref="ISerializer"/> instances.
    /// </summary>
    public class NewtonsoftJsonSerializerProvider : ISerializerProvider
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        /// <summary>
        /// Creates the Newtonsoft.Json based serializer provider.
        /// </summary>
        public NewtonsoftJsonSerializerProvider()
        {
            _jsonSerializerSettings = new JsonSerializerSettings();
        }

        /// <summary>
        /// Creates the Newtonsoft.Json based serializer provider.
        /// </summary>
        /// <param name="jsonSerializerSettings">The settings to be used with <see cref="JsonSerializer"/>.</param>
        public NewtonsoftJsonSerializerProvider(JsonSerializerSettings jsonSerializerSettings)
        {
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));

            _jsonSerializerSettings = jsonSerializerSettings;
        }

        public ISerializer Create(ILogger? logger)
        {
            return new NewtonsoftJsonSerializer(_jsonSerializerSettings);
        }
    }
}
