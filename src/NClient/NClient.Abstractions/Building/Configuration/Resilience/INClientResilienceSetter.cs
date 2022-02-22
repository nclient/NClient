using NClient.Providers.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientResilienceSetter<TClient, TRequest, TResponse>
    {
        INClientResilienceMethodSelector<TClient, TRequest, TResponse> Use(IResiliencePolicy<TRequest, TResponse> policy);
        INClientResilienceMethodSelector<TClient, TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> provider);
        INClientResilienceMethodSelector<TClient, TRequest, TResponse> DoNotUse();
    }
}
