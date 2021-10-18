using NClient.Abstractions.Building;
using NClient.Abstractions.Resilience;
using NClient.Common.Helpers;
using NClient.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class FullResilienceExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithFullResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder, 
            IResiliencePolicyProvider<TRequest, TResponse> provider) 
            where TClient : class
        {
            Ensure.IsNotNull(provider, nameof(provider));
            
            return clientOptionalBuilder.WithCustomResilience(new MethodResiliencePolicyProviderAdapter<TRequest, TResponse>(provider));
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithFullResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder, 
            IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            Ensure.IsNotNull(provider, nameof(provider));
            
            return factoryOptionalBuilder.WithCustomResilience(new MethodResiliencePolicyProviderAdapter<TRequest, TResponse>(provider));
        }
    }
}
