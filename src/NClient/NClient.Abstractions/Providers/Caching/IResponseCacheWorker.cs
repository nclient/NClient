using System;
using System.Threading;
using System.Threading.Tasks;

namespace NClient.Providers.Caching
{
    /// <summary>The worker that manage caching of responses.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface IResponseCacheWorker<TRequest, TResponse>
    {
        public Task<TResponse?> FindAsync(TRequest request, CancellationToken cancellationToken = default);
        public Task PutAsync(TRequest request, TResponse response, TimeSpan? lifeTime = null, CancellationToken cancellationToken = default);
    }
}
