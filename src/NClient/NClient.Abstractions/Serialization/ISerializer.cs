using System;

namespace NClient.Abstractions.Serialization
{
    public interface ISerializer
    {
        object? Deserialize(string json, Type returnType);
        string Serialize<T>(T? value);
    }
}