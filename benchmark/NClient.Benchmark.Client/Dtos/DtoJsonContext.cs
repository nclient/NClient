using System.Text.Json.Serialization;

namespace NClient.Benchmark.Client.Dtos
{
    [JsonSerializable(typeof(Dto))]
    public partial class DtoJsonContext : JsonSerializerContext
    {
    }
}
