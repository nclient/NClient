using System.Text.Json.Serialization;

namespace NClient.Testing.Common.Entities
{
    public class BasicEntityWithCustomJsonName
    {
        [JsonPropertyName("MyId")]
        public int Id { get; set; }
        public int Value { get; set; }
    }
}
