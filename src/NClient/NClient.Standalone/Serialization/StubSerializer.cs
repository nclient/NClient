using System;
using NClient.Abstractions.Serialization;

namespace NClient.Core.Serialization
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
