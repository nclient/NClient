using NClient.Providers.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>Setter for custom functionality to providing resilient requests.</summary>
    public interface INClientResilienceSetter<TClient, TRequest, TResponse>
    {
        /// <summary>Sets exception handling policies.</summary>
        /// <param name="policy">Transient exception handling policies.</param>
        INClientResilienceMethodSelector<TClient, TRequest, TResponse> Use(IResiliencePolicy<TRequest, TResponse> policy);
        
        /// <summary>Sets the provider that can create a transient exception handling policies.</summary>
        /// <param name="provider">The provider that can create a transient exception handling policies.</param>
        INClientResilienceMethodSelector<TClient, TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> provider);
        
        /// <summary>Removes any exception handling policies.</summary>
        INClientResilienceMethodSelector<TClient, TRequest, TResponse> DoNotUse();
    }
}
