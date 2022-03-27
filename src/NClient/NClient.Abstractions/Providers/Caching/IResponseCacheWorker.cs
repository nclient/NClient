using System;
using System.Threading;
using System.Threading.Tasks;

namespace NClient.Providers.Caching
{
    /// <summary>The worker that manage caching of responses.</summary>
    public interface IResponseCacheWorker
    {
        public Task<TResponse?> FindAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default);
        public Task PutAsync<TRequest, TResponse>(TRequest request, TResponse response, TimeSpan? lifeTime = null, CancellationToken cancellationToken = default);
    }
}
