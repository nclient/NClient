using System;
using System.Threading.Tasks;
using NClient.Providers.Resilience;
using NClient.Providers.Serialization;

namespace NClient.Providers.Results
{
    public interface IResultBuilder<TRequest, TResponse>
    {
        bool CanBuild(Type resultType, IResponseContext<TRequest, TResponse> responseContext);
        Task<object?> BuildAsync(Type resultType, IResponseContext<TRequest, TResponse> responseContext, ISerializer serializer);
    }
}
