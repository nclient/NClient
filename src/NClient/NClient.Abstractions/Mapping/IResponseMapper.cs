using System;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;

namespace NClient.Abstractions.Mapping
{
    public interface IResponseMapper<TResponse>
    {
        bool CanMapTo(Type resultType);
        Task<object?> MapAsync(Type resultType, IHttpRequest httpRequest, TResponse response, ISerializer serializer);
    }
}
