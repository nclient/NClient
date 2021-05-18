using System.Text.Json;
using NClient.Abstractions.Serialization;

namespace NClient.Providers.Serialization.System
{
    public class SystemSerializerProvider : ISerializerProvider
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public SystemSerializerProvider()
        {
            _jsonSerializerOptions = new JsonSerializerOptions();
        }

        public SystemSerializerProvider(JsonSerializerOptions jsonSerializerOptions)
        {
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public ISerializer Create()
        {
            return new SystemSerializer(_jsonSerializerOptions);
        }
    }
}