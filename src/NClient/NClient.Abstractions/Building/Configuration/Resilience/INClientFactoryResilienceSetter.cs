using NClient.Providers.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient
{
    // TODO: doc
    public interface INClientFactoryResilienceSetter<TRequest, TResponse>
    {
        INClientFactoryResilienceMethodSelector<TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);
        INClientFactoryResilienceMethodSelector<TRequest, TResponse> DoNotUse();
    }
}
