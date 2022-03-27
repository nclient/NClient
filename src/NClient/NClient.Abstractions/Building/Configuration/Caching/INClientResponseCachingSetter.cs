using NClient.Providers.Caching;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>Setter for custom functionality to mapping NClient requests.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface INClientResponseCachingSetter<TRequest, TResponse>
    {
        /// <summary>Sets a custom mappers that can convert NClient responses into custom ones.</summary>
        /// <param name="cacheWorkers">The mappers that convert transport responses into custom results.</param>
        INClientResponseCachingSelector<TRequest, TResponse> Use(IResponseCacheWorker cacheWorkers);

        /// <summary>Sets a providers creating custom mappers that can convert NClient responses into custom ones.</summary>
        /// <param name="cacheProvider">The providers of a mappers that convert transport responses into custom results.</param>
        INClientResponseCachingSelector<TRequest, TResponse> Use(IResponseCacheProvider cacheProvider);
    }
}
