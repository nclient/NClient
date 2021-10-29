using System;
using NClient.Providers.Serialization;

namespace NClient.Standalone.ClientProxy.Validation.Serialization
{
    internal class StubSerializer : ISerializer
    {
        public string ContentType { get; } = "application/json";

        public object? Deserialize(string source, Type returnType)
        {
            return null;
        }

        public string Serialize<T>(T? value)
        {
            return "";
        }
    }
}
