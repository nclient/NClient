using NClient.Providers.Caching;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ExtraCachingExtensions
    {
        /// <summary>Sets the mappers that convert NClient responses into custom results.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="responseCacheWorker">The responseCacheWorker that converts transport responses into custom results.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithResponseCaching<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            IResponseCacheWorker<TRequest, TResponse> responseCacheWorker) 
            where TClient : class
        {
            return optionalBuilder.WithResponseCaching(responseCacheWorker);
        }

        /// <summary>Sets the mappers that convert NClient responses into custom results.</summary>
        /// <param name="responseCachingSetter"></param>
        /// <param name="responseCacheWorker">The responseCacheWorker that converts transport responses into custom results.</param>
        public static INClientResponseCachingSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientResponseCachingSetter<TRequest, TResponse> responseCachingSetter,
            IResponseCacheWorker<IRequest, IResponse> responseCacheWorker)
        {
            return responseCachingSetter.Use(responseCacheWorker);
        }

        /// <summary>Sets the mappers that convert NClient responses into custom results.</summary>
        /// <param name="transportResponseCachingSetter"></param>
        /// <param name="responseCacheWorker">The responseCacheWorker that converts transport responses into custom results.</param>
        public static INClientResponseCachingSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportResponseCachingSetter<TRequest, TResponse> transportResponseCachingSetter,
            IResponseCacheWorker<TRequest, TResponse> responseCacheWorker)
        {
            return transportResponseCachingSetter.Use(responseCacheWorker);
        }
    }
}
