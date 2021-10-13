using System;
using NClient.Abstractions.Serialization;

namespace NClient.Abstractions.Results
{
    public interface IResultBuilder<TResponse>
    {
        bool CanBuild(Type resultType, TResponse response);
        object? Build(Type resultType, TResponse response, ISerializer serializer);
    }
}
