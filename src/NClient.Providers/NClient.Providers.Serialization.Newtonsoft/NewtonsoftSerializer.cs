using System;
using NClient.Abstractions.Serialization;
using Newtonsoft.Json;

namespace NClient.Providers.Serialization.Newtonsoft
{
    public class NewtonsoftSerializer : ISerializer
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public NewtonsoftSerializer(JsonSerializerSettings jsonSerializerSettings)
        {
            _jsonSerializerSettings = jsonSerializerSettings;
        }

        public object? Deserialize(string json, Type returnType)
        {
            return JsonConvert.DeserializeObject(json, returnType, _jsonSerializerSettings);
        }

        public string Serialize<T>(T? value)
        {
            return JsonConvert.SerializeObject(value, _jsonSerializerSettings);
        }
    }
}