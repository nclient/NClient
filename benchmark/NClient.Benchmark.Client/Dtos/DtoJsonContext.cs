using System.Text.Json.Serialization;

namespace NClient.Benchmark.Client.Dtos
{
    [JsonSourceGenerationOptions]
    [JsonSerializable(typeof(Dto))]
    internal partial class DtoJsonContext : JsonSerializerContext
    {
    }
}
