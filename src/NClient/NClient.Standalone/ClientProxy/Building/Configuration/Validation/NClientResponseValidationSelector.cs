using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Validation
{
    internal class NClientResponseValidationSelector<TRequest, TResponse> : INClientResponseValidationSelector<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        
        public NClientResponseValidationSelector(BuilderContextModifier<TRequest, TResponse> builderContextModifier)
        {
            _builderContextModifier = builderContextModifier;
        }
        
        public INClientTransportResponseValidationSetter<TRequest, TResponse> ForTransport()
        {
            return new NClientTransportResponseValidationSetter<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
