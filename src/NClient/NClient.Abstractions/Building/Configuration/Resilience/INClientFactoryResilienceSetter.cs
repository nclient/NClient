using NClient.Abstractions.Providers.Resilience;

namespace NClient.Abstractions.Building.Configuration.Resilience
{
    // TODO: doc
    public interface INClientFactoryResilienceSetter<TRequest, TResponse>
    {
        INClientFactoryResilienceMethodSelector<TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);
        INClientFactoryResilienceMethodSelector<TRequest, TResponse> DoNotUse();
    }
}
