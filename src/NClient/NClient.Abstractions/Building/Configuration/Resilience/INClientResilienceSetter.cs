using NClient.Providers.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient
{
    // TODO: doc
    public interface INClientResilienceSetter<TClient, TRequest, TResponse>
    {
        INClientResilienceMethodSelector<TClient, TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);
        INClientResilienceMethodSelector<TClient, TRequest, TResponse> DoNotUse();
    }
}
