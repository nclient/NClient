using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Mapping
{
    internal class NClientResponseMappingSelector<TRequest, TResponse> : INClientResponseMappingSelector<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        
        public NClientResponseMappingSelector(BuilderContextModifier<TRequest, TResponse> builderContextModifier)
        {
            _builderContextModifier = builderContextModifier;
        }
        
        public INClientResponseMappingSetter<TRequest, TResponse> ForClient()
        {
            return new NClientResponseMappingSetter<TRequest, TResponse>(_builderContextModifier);
        }
        
        public INClientTransportResponseMappingSetter<TRequest, TResponse> ForTransport()
        {
            return new NClientTransportResponseMappingSetter<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
