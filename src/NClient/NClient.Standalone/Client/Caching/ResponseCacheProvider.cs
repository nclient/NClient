using NClient.Providers;
using NClient.Providers.Caching;

namespace NClient.Standalone.Client.Caching
{
    internal class ResponseCacheProvider<TRequest, TResponse> : IResponseCacheProvider<TRequest, TResponse>
    {
        private readonly IResponseCacheWorker<TRequest, TResponse> _responseCacheWorker;
        
        public ResponseCacheProvider(IResponseCacheWorker<TRequest, TResponse> responseCacheWorker)
        {
            _responseCacheWorker = responseCacheWorker;
        }
        
        public IResponseCacheWorker<TRequest, TResponse> Create(IToolset toolset)
        {
            return _responseCacheWorker;
        }
    }
}
