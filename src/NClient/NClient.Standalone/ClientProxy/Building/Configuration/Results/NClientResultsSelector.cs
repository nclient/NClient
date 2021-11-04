using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Results
{
    internal class NClientResultsSelector<TRequest, TResponse> : INClientResultsSelector<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        
        public NClientResultsSelector(BuilderContextModifier<TRequest, TResponse> builderContextModifier)
        {
            _builderContextModifier = builderContextModifier;
        }
        
        public INClientResultsSetter<TRequest, TResponse> ForClient()
        {
            return new NClientResultsSetter<TRequest, TResponse>(_builderContextModifier);
        }
        
        public INClientTransportResultsSetter<TRequest, TResponse> ForTransport()
        {
            return new NClientTransportResultsSetter<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
