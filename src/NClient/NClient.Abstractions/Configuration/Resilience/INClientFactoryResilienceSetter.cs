using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Configuration.Resilience
{
    // TODO: doc
    public interface INClientFactoryResilienceSetter<TRequest, TResponse>
    {
        INClientFactoryResilienceMethodSelector<TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);
        INClientFactoryResilienceMethodSelector<TRequest, TResponse> DoNotUse();
    }
}
