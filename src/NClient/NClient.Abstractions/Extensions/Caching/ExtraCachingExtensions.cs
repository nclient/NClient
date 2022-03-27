using System;
using System.Threading;
using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Providers.Caching;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ExtraCachingExtensions
    {
        /// <summary>Sets the mappers that convert NClient responses into custom results.</summary>
        /// <param name="responseCachingSetter"></param>
        /// <param name="responseCacheWorker">The responseCacheWorker that converts transport responses into custom results.</param>
        public static INClientResponseCachingSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientResponseCachingSetter<TRequest, TResponse> responseCachingSetter,
            IResponseCacheWorker responseCacheWorker)
        {
            return responseCachingSetter.Use(responseCacheWorker);
        }

        /// <summary>Sets the mappers that convert NClient responses into custom results.</summary>
        /// <param name="transportResponseCachingSetter"></param>
        /// <param name="responseCacheWorker">The responseCacheWorker that converts transport responses into custom results.</param>
        public static INClientResponseCachingSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportResponseCachingSetter<TRequest, TResponse> transportResponseCachingSetter,
            IResponseCacheWorker responseCacheWorker)
        {
            return transportResponseCachingSetter.Use(responseCacheWorker);
        }

        public static async Task Put(this IResponseCacheWorker? worker, IRequest request, IResponse response, ICachingAttribute? cachingAttribute, CancellationToken cancellationToken = default)
        {
            if (worker is not null)
                await worker.PutAsync(request, response, TimeSpan.FromMilliseconds(cachingAttribute?.Milliseconds ?? 0), cancellationToken);
        }
        
        public static async Task<IResponse?> TryGet(this IResponseCacheWorker? worker, IRequest request, CancellationToken cancellationToken = default)
        {
            if (worker is null)
                return null;
            return await worker.FindAsync(request, cancellationToken);
        }
    }
}
