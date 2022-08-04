using NClient.Providers;
using NClient.Providers.Caching;

namespace NClient.Standalone.Client.Caching
{
    internal class ResponseCacheProvider : IResponseCacheProvider
    {
        private readonly IResponseCacheWorker _responseCacheWorker;
        
        public ResponseCacheProvider(IResponseCacheWorker responseCacheWorker)
        {
            _responseCacheWorker = responseCacheWorker;
        }
        
        public IResponseCacheWorker Create(IToolset toolset)
        {
            return _responseCacheWorker;
        }
    }
}
