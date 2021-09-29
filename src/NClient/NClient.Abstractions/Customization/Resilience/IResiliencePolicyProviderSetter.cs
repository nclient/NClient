using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Customization.Resilience
{
    // TODO: doc
    public interface IResiliencePolicyProviderSetter<TInterface, TRequest, TResponse>
    {
        IResiliencePolicyMethodSelector<TInterface, TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> provider);
    }
    
    // TODO: doc
    public interface IResiliencePolicyProviderSetter<TRequest, TResponse>
    {
        IResiliencePolicyMethodSelector<TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> provider);
    }
}
