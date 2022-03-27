using System;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Caching;

namespace NClient.Standalone.ClientProxy.Validation.Caching
{
    internal class StubResponseCacheWorker : IResponseCacheWorker
    {
        public async Task<TResponse?> FindAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult<TResponse?>(default);
        }
        public async Task PutAsync<TRequest, TResponse>(TRequest request, TResponse response, TimeSpan? lifeTime = null, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }
    }
}
