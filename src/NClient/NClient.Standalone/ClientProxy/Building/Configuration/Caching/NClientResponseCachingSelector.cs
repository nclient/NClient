using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Caching
{
    internal class NClientResponseCachingSelector<TRequest, TResponse> : INClientResponseCachingSelector<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;

        public NClientResponseCachingSelector(BuilderContextModifier<TRequest, TResponse> builderContextModifier)
        {
            _builderContextModifier = builderContextModifier;
        }

        public INClientResponseCachingSetter<TRequest, TResponse> ForClient()
        {
            return new NClientResponseCachingSetter<TRequest, TResponse>(_builderContextModifier);
        }

        public INClientTransportResponseCachingSetter<TRequest, TResponse> ForTransport()
        {
            return new NClientTransportResponseCachingSetter<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
