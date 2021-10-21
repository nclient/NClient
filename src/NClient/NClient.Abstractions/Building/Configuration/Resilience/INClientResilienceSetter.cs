using NClient.Abstractions.Providers.Resilience;

namespace NClient.Abstractions.Building.Configuration.Resilience
{
    // TODO: doc
    public interface INClientResilienceSetter<TClient, TRequest, TResponse>
    {
        INClientResilienceMethodSelector<TClient, TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);
        INClientResilienceMethodSelector<TClient, TRequest, TResponse> DoNotUse();
    }
}
