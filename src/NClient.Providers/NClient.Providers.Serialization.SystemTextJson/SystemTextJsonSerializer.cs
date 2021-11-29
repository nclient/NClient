using System;
using System.Text.Json;
using NClient.Common.Helpers;

namespace NClient.Providers.Serialization.SystemTextJson
{
    internal class SystemTextJsonSerializer : ISerializer
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public string ContentType { get; } = "application/json";

        public SystemTextJsonSerializer(JsonSerializerOptions jsonSerializerOptions)
        {
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public object? Deserialize(string source, Type returnType)
        {
            Ensure.IsNotNull(source, nameof(source));
            Ensure.IsNotNull(returnType, nameof(returnType));

            return JsonSerializer.Deserialize(source, returnType, _jsonSerializerOptions);
        }

        public string Serialize<T>(T? value)
        {
            return JsonSerializer.Serialize(value, _jsonSerializerOptions);
        }
    }
}
