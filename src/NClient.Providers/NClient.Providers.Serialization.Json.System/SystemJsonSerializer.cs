using System;
using System.Text.Json;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;

namespace NClient.Providers.Serialization.Json.System
{
    internal class SystemJsonSerializer : ISerializer
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public string ContentType { get; } = "application/json";

        public SystemJsonSerializer(JsonSerializerOptions jsonSerializerOptions)
        {
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public object? Deserialize(string json, Type returnType)
        {
            Ensure.IsNotNull(json, nameof(json));
            Ensure.IsNotNull(returnType, nameof(returnType));

            return JsonSerializer.Deserialize(json, returnType, _jsonSerializerOptions);
        }

        public string Serialize<T>(T? value)
        {
            return JsonSerializer.Serialize(value, _jsonSerializerOptions);
        }
    }
}
