using System;
using NClient.Abstractions.Providers.Serialization;
using NClient.Common.Helpers;
using Newtonsoft.Json;

namespace NClient.Providers.Serialization.Json.Newtonsoft
{
    internal class NewtonsoftJsonSerializer : ISerializer
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public string ContentType { get; } = "application/json";

        public NewtonsoftJsonSerializer(JsonSerializerSettings jsonSerializerSettings)
        {
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));

            _jsonSerializerSettings = jsonSerializerSettings;
        }

        public object? Deserialize(string source, Type returnType)
        {
            Ensure.IsNotNull(source, nameof(source));
            Ensure.IsNotNull(returnType, nameof(returnType));

            return JsonConvert.DeserializeObject(source, returnType, _jsonSerializerSettings);
        }

        public string Serialize<T>(T? value)
        {
            return JsonConvert.SerializeObject(value, _jsonSerializerSettings);
        }
    }
}
