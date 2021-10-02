using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Customization.Resilience
{
    // TODO: doc
    public interface IResiliencePolicyProviderSetter<TClient, TRequest, TResponse>
    {
        IResiliencePolicyMethodSelector<TClient, TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> provider);
    }
    
    // TODO: doc
    public interface IResiliencePolicyProviderSetter<TRequest, TResponse>
    {
        IResiliencePolicyMethodSelector<TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> provider);
    }
}
