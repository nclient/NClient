using System;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using Newtonsoft.Json;

namespace NClient.Providers.Serialization.Newtonsoft
{
    internal class NewtonsoftSerializer : ISerializer
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public string ContentType { get; } = "application/json";

        public NewtonsoftSerializer(JsonSerializerSettings jsonSerializerSettings)
        {
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));

            _jsonSerializerSettings = jsonSerializerSettings;
        }

        public object? Deserialize(string json, Type returnType)
        {
            Ensure.IsNotNull(json, nameof(json));
            Ensure.IsNotNull(returnType, nameof(returnType));

            return JsonConvert.DeserializeObject(json, returnType, _jsonSerializerSettings);
        }

        public string Serialize<T>(T? value)
        {
            return JsonConvert.SerializeObject(value, _jsonSerializerSettings);
        }
    }
}
