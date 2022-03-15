#if NET6_0_OR_GREATER
using System.Text.Json.Serialization;
using NClient.Testing.Common.Entities;

namespace NClient.Providers.Serialization.SystemTextJson.Tests.Contexts
{
    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    [JsonSerializable(typeof(BasicEntity))]
    internal partial class BasicEntityJsonContext : JsonSerializerContext
    {
    }
}
#endif
