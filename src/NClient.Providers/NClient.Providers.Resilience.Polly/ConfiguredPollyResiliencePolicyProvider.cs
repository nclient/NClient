using System;
using System.Net.Http;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using Polly;

namespace NClient.Providers.Resilience.Polly
{
    public class ConfiguredPollyResiliencePolicyProvider<TRequest, TResponse> : IResiliencePolicyProvider<TRequest, TResponse>
    {
        private readonly IResiliencePolicySettings<TRequest, TResponse> _settings;
        
        public ConfiguredPollyResiliencePolicyProvider(IResiliencePolicySettings<TRequest, TResponse> settings)
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
            var fallbackPolicy = basePolicy.FallbackAsync(
                fallbackAction: (delegateResult, _, _) => Task.FromResult(delegateResult.Result),
                onFallbackAsync: (delegateResult, _) =>
                {
                    if (delegateResult.Exception is not null)
                        throw delegateResult.Exception;
                    if (delegateResult.Result.MethodInvocation.ResultType == typeof(HttpResponseMessage))
                        return Task.CompletedTask;
                    if (typeof(HttpResponse).IsAssignableFrom(delegateResult.Result.MethodInvocation.ResultType))
                        return Task.CompletedTask;
                    return _settings.OnFallbackAsync(delegateResult.Result);
                });
            
            return new PollyResiliencePolicy<TRequest, TResponse>(fallbackPolicy.WrapAsync(retryPolicy));
        }
    }
}
