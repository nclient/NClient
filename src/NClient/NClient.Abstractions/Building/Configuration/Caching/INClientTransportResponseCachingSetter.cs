using NClient.Providers.Caching;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>Setter for custom functionality to mapping transport requests.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface INClientTransportResponseCachingSetter<TRequest, TResponse>
    {
        /// <summary>Sets a custom mappers that can convert NClient responses into custom ones.</summary>
        /// <param name="cacheWorker">The mapper that convert transport responses into custom results.</param>
        INClientResponseCachingSelector<TRequest, TResponse> Use(IResponseCacheWorker<TRequest, TResponse> cacheWorker);
        
        /// <summary>Sets a providers creating custom mappers that can convert NClient responses into custom ones.</summary>
        /// <param name="cacheProvider">The provider of a mappers that convert transport responses into custom results.</param>
        INClientResponseCachingSelector<TRequest, TResponse> Use(IResponseCacheProvider<TRequest, TResponse> cacheProvider);
    }
}
