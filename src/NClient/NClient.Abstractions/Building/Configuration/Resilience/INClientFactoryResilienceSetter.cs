using NClient.Providers.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>Setter for custom functionality to providing resilient requests.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface INClientFactoryResilienceSetter<TRequest, TResponse>
    {
        /// <summary>Sets exception handling policies.</summary>
        /// <param name="policy">Transient exception handling policies.</param>
        INClientFactoryResilienceMethodSelector<TRequest, TResponse> Use(IResiliencePolicy<TRequest, TResponse> policy);
        
        /// <summary>Sets the provider that can create a transient exception handling policies.</summary>
        /// <param name="provider">The provider that can create a transient exception handling policies.</param>
        INClientFactoryResilienceMethodSelector<TRequest, TResponse> Use(IResiliencePolicyProvider<TRequest, TResponse> provider);
        
        /// <summary>Removes any exception handling policies.</summary>
        INClientFactoryResilienceMethodSelector<TRequest, TResponse> DoNotUse();
    }
}
