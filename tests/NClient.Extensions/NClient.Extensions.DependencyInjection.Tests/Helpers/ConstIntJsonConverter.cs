using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NClient.Extensions.DependencyInjection.Tests.Helpers
{
    public class ConstIntJsonConverter : JsonConverter<int>
    {
        public int Value => int.MaxValue;

        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Value;
        }
        
        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
