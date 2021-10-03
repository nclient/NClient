using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Customization.Resilience
{
    // TODO: doc
    public interface INClientResilienceSetter<TClient, TRequest, TResponse>
    {
        INClientResilienceMethodSelector<TClient, TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);
    }
}
