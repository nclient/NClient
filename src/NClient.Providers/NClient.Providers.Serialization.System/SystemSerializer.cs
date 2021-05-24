using System;
using System.Text.Json;
using NClient.Abstractions.Serialization;

namespace NClient.Providers.Serialization.System
{
    public class SystemSerializer : ISerializer
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public SystemSerializer(JsonSerializerOptions jsonSerializerOptions)
        {
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public object? Deserialize(string json, Type returnType)
        {
            return JsonSerializer.Deserialize(json, returnType, _jsonSerializerOptions);
        }

        public string Serialize<T>(T? value)
        {
            return JsonSerializer.Serialize(value, _jsonSerializerOptions);
        }
    }
}