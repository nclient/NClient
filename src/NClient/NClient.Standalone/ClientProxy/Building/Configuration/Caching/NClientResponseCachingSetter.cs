using NClient.Common.Helpers;
using NClient.Providers.Caching;
using NClient.Providers.Transport;
using NClient.Standalone.Client.Caching;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Caching
{
    internal class NClientResponseCachingSetter<TRequest, TResponse> : INClientResponseCachingSetter<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        
        public NClientResponseCachingSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier)
        {
            _builderContextModifier = builderContextModifier;
        }

        public INClientResponseCachingSelector<TRequest, TResponse> Use(IResponseCacheWorker<IRequest, IResponse> cacheWorker)
        {
            Ensure.IsNotNull(cacheWorker, nameof(cacheWorker));
            
            return Use(new ResponseCacheProvider<IRequest, IResponse>(cacheWorker));
        }
        
        public INClientResponseCachingSelector<TRequest, TResponse> Use(IResponseCacheProvider<IRequest, IResponse> cacheProvider)
        {
            Ensure.IsNotNull(cacheProvider, nameof(cacheProvider));
            
            _builderContextModifier.Add(x => x.WithResponseCachingProvider(cacheProvider));
            return new NClientResponseCachingSelector<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
