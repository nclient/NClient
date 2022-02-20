using System;
using NClient.Providers.Transport;
using Polly;

namespace NClient.Providers.Resilience.Polly
{
    /// <summary>The default Polly based resilience policy.</summary>
    public class DefaultPollyResiliencePolicyProvider<TRequest, TResponse> : IResiliencePolicyProvider<TRequest, TResponse>
    {
        private readonly IResiliencePolicySettings<TRequest, TResponse> _settings;
        
        /// <summary>Creates the default Polly based resilience policy.</summary>
        /// <param name="settings">The settings for resilience policy provider.</param>
        public DefaultPollyResiliencePolicyProvider(IResiliencePolicySettings<TRequest, TResponse> settings)
        {
            _settings = settings;
        }
        
        /// <summary>Creates the default Polly based resilience policy.</summary>
        /// <param name="toolset">Tools that help implement providers.</param>
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
