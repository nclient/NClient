using System;
using NClient.Providers.Transport;
using Polly;

namespace NClient.Providers.Resilience.Polly
{
    public class DefaultPollyResiliencePolicyProvider<TRequest, TResponse> : IResiliencePolicyProvider<TRequest, TResponse>
    {
        private readonly IResiliencePolicySettings<TRequest, TResponse> _settings;
        
        public DefaultPollyResiliencePolicyProvider(IResiliencePolicySettings<TRequest, TResponse> settings)
        {
            _settings = settings;
        }
        
        public IResiliencePolicy<TRequest, TResponse> Create(IToolset toolset)
        {
            var basePolicy = Policy<IResponseContext<TRequest, TResponse>>
                .HandleResult(_settings.ShouldRetry).Or<Exception>();

            var retryPolicy = basePolicy.WaitAndRetryAsync(
                _settings.MaxRetries,
                _settings.GetDelay);
            
            return new PollyResiliencePolicy<TRequest, TResponse>(retryPolicy);
        }
    }
}
