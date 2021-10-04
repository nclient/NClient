using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Customization.Resilience
{
    // TODO: doc
    public interface INClientFactoryResilienceSetter<TRequest, TResponse>
    {
        INClientFactoryResilienceMethodSelector<TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);
        INClientFactoryResilienceMethodSelector<TRequest, TResponse> DoNotUse();
    }
}
