using System;
using NClient.Abstractions.Resilience;
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
        
        public IResiliencePolicy<TRequest, TResponse> Create()
        {
            var basePolicy = Policy<ResponseContext<TRequest, TResponse>>
                .HandleResult(_settings.ResultPredicate).Or<Exception>();

            var retryPolicy = basePolicy.WaitAndRetryAsync(
                _settings.RetryCount,
                _settings.SleepDuration);
            
            return new PollyResiliencePolicy<TRequest, TResponse>(retryPolicy);
        }
    }
}
