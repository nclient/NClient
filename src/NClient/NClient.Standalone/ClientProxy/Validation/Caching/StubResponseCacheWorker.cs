using System;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Caching;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validation.Caching
{
    internal class StubResponseCacheWorker : IResponseCacheWorker
    {
        public async Task<IResponse?> FindAsync(IRequest request, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult<IResponse?>(default);
        }
        public async Task PutAsync(IRequest request, IResponse response, TimeSpan? lifeTime = null, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }
    }
}
