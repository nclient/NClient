using NClient.Common.Helpers;
using NClient.Providers.Caching;
using NClient.Standalone.Client.Caching;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Caching
{
    internal class NClientTransportResponseCachingSetter<TRequest, TResponse> : INClientTransportResponseCachingSetter<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        
        public NClientTransportResponseCachingSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier)
        {
            _builderContextModifier = builderContextModifier;
        }
        
        public INClientResponseCachingSelector<TRequest, TResponse> Use(IResponseCacheWorker<TRequest, TResponse> cacheWorker)
        {
            Ensure.IsNotNull(cacheWorker, nameof(cacheWorker));

            return Use(new ResponseCacheProvider<TRequest, TResponse>(cacheWorker));
        }
        
        public INClientResponseCachingSelector<TRequest, TResponse> Use(IResponseCacheProvider<TRequest, TResponse> cacheProvider)
        {
            Ensure.IsNotNull(cacheProvider, nameof(cacheProvider));
            
            _builderContextModifier.Add(x => x.WithTransportCachingProvider(cacheProvider));
            return new NClientResponseCachingSelector<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
