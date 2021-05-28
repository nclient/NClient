using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using Newtonsoft.Json;

namespace NClient.Providers.Serialization.Newtonsoft
{
    public class NewtonsoftSerializerProvider : ISerializerProvider
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public NewtonsoftSerializerProvider()
        {
            _jsonSerializerSettings = new JsonSerializerSettings();
        }

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