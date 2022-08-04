using System;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Transport;

namespace NClient.Providers.Caching
{
    /// <summary>The worker that manage caching of responses.</summary>
    public interface IResponseCacheWorker
    {
        public Task<IResponse?> FindAsync(IRequest request, CancellationToken cancellationToken = default);
        public Task PutAsync(IRequest request, IResponse response, TimeSpan? lifeTime = null, CancellationToken cancellationToken = default);
    }
}
