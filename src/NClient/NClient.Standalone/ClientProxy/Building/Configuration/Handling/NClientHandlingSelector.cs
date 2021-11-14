using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Handling
{
    internal class NClientHandlingSelector<TRequest, TResponse> : INClientHandlingSelector<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        
        public NClientHandlingSelector(BuilderContextModifier<TRequest, TResponse> builderContextModifier)
        {
            _builderContextModifier = builderContextModifier;
        }
        
        public INClientTransportHandlingSetter<TRequest, TResponse> ForTransport()
        {
            return new NClientTransportHandlingSetter<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
