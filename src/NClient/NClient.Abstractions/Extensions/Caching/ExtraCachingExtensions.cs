using NClient.Providers.Caching;

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
    }
}
