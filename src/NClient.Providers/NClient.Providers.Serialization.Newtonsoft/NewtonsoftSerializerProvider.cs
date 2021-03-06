using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using Newtonsoft.Json;

namespace NClient.Providers.Serialization.Newtonsoft
{
    /// <summary>
    /// The Newtonsoft.Json based provider for a component that can create <see cref="ISerializer"/> instances.
    /// </summary>
    public class NewtonsoftSerializerProvider : ISerializerProvider
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        /// <summary>
        /// Creates the Newtonsoft.Json based serializer provider.
        /// </summary>
        public NewtonsoftSerializerProvider()
        {
            _jsonSerializerSettings = new JsonSerializerSettings();
        }

        /// <summary>
        /// Creates the Newtonsoft.Json based serializer provider.
        /// </summary>
        /// <param name="jsonSerializerSettings">The settings to be used with <see cref="JsonSerializer"/>.</param>
        public NewtonsoftSerializerProvider(JsonSerializerSettings jsonSerializerSettings)
        {
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));

            _jsonSerializerSettings = jsonSerializerSettings;
        }

        public ISerializer Create()
        {
            return new NewtonsoftSerializer(_jsonSerializerSettings);
        }
    }
}