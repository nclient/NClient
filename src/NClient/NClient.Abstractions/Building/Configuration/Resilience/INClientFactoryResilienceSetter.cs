using NClient.Providers.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient
{
    // TODO: doc
    public interface INClientFactoryResilienceSetter<TRequest, TResponse>
    {
        INClientFactoryResilienceMethodSelector<TRequest, TResponse> Use(IResiliencePolicy<TRequest, TResponse> policy);
        INClientFactoryResilienceMethodSelector<TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> provider);
        INClientFactoryResilienceMethodSelector<TRequest, TResponse> DoNotUse();
    }
}
