using System;
using System.Threading.Tasks;
using NClient.Providers.Serialization;
using NClient.Providers.Transport;

namespace NClient.Providers.Mapping
{
    public interface IResponseMapper<TRequest, TResponse>
    {
        bool CanMap(Type resultType, IResponseContext<TRequest, TResponse> responseContext);
        Task<object?> MapAsync(Type resultType, IResponseContext<TRequest, TResponse> responseContext, ISerializer serializer);
    }
}
