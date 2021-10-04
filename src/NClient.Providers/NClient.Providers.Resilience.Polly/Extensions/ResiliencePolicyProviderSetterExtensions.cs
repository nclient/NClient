using System;
using NClient.Abstractions.Customization.Resilience;
using NClient.Abstractions.Resilience;
using Polly;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Resilience.Polly
{
    public static class ResiliencePolicyProviderSetterExtensions
    {
        public static INClientResilienceMethodSelector<TClient, TRequest, TResponse> UsePolly<TClient, TRequest, TResponse>(
            this INClientResilienceSetter<TClient, TRequest, TResponse> clientResilienceSetter, 
            IResiliencePolicySettings<TRequest, TResponse> policySettings)
        {
            return clientResilienceSetter.Use(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(policySettings));
        }
        
        public static INClientFactoryResilienceMethodSelector<TRequest, TResponse> UsePolly<TRequest, TResponse>(
            this INClientFactoryResilienceSetter<TRequest, TResponse> factoryResilienceSetter, 
            IResiliencePolicySettings<TRequest, TResponse> policySettings)
        {
            return factoryResilienceSetter.Use(new DefaultPollyResiliencePolicyProvider<TRequest, TResponse>(policySettings));
        }
        
        public static INClientResilienceMethodSelector<TClient, TRequest, TResponse> UsePolly<TClient, TRequest, TResponse>(
            this INClientResilienceSetter<TClient, TRequest, TResponse> clientResilienceSetter, 
            int maxRetries, Func<int, TimeSpan> getDelay, Func<ResponseContext<TRequest, TResponse>, bool> shouldRetry)
        {
            return clientResilienceSetter.UsePolly(
                new ResiliencePolicySettings<TRequest, TResponse>(maxRetries, getDelay, shouldRetry));
        }
        
        public static INClientFactoryResilienceMethodSelector<TRequest, TResponse> UsePolly<TRequest, TResponse>(
            this INClientFactoryResilienceSetter<TRequest, TResponse> factoryResilienceSetter, 
            int maxRetries, Func<int, TimeSpan> getDelay, Func<ResponseContext<TRequest, TResponse>, bool> shouldRetry)
        {
            return factoryResilienceSetter.UsePolly(
                new ResiliencePolicySettings<TRequest, TResponse>(maxRetries, getDelay, shouldRetry));
        }
        
        // TODO: doc
        public static INClientResilienceMethodSelector<TClient, TRequest, TResponse> UsePolly<TClient, TRequest, TResponse>(
            this INClientResilienceSetter<TClient, TRequest, TResponse> clientResilienceSetter, 
            IAsyncPolicy<ResponseContext<TRequest, TResponse>> asyncPolicy)
        {
            return clientResilienceSetter.Use(new PollyResiliencePolicyProvider<TRequest, TResponse>(asyncPolicy));
        }

        public static INClientFactoryResilienceMethodSelector<TRequest, TResponse> UsePolly<TRequest, TResponse>(
            this INClientFactoryResilienceSetter<TRequest, TResponse> factoryResilienceSetter, 
            IAsyncPolicy<ResponseContext<TRequest, TResponse>> asyncPolicy)
        {
            return factoryResilienceSetter.Use(new PollyResiliencePolicyProvider<TRequest, TResponse>(asyncPolicy));
        }
    }
}
