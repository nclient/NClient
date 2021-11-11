using NClient.Providers;
using NClient.Providers.Resilience;

namespace NClient.Standalone.Client.Resilience
{
    internal class ResiliencePolicyProvider<TRequest, TResponse> : IResiliencePolicyProvider<TRequest, TResponse>
    {
        private readonly IResiliencePolicy<TRequest, TResponse> _resiliencePolicy;
        
        public ResiliencePolicyProvider(IResiliencePolicy<TRequest, TResponse> resiliencePolicy)
        {
            _resiliencePolicy = resiliencePolicy;
        }
        
        public IResiliencePolicy<TRequest, TResponse> Create(IToolSet toolset)
        {
            return _resiliencePolicy;
        }
    }
}
