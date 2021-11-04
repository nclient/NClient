using System;
using NClient.Providers.Resilience;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Transport;
using Polly;

// ReSharper disable once CheckNamespace
namespace NClient
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
            int maxRetries, Func<int, TimeSpan> getDelay, Func<IResponseContext<TRequest, TResponse>, bool> shouldRetry)
        {
            return clientResilienceSetter.UsePolly(
                new ResiliencePolicySettings<TRequest, TResponse>(maxRetries, getDelay, shouldRetry));
        }
        
        public static INClientFactoryResilienceMethodSelector<TRequest, TResponse> UsePolly<TRequest, TResponse>(
            this INClientFactoryResilienceSetter<TRequest, TResponse> factoryResilienceSetter, 
            int maxRetries, Func<int, TimeSpan> getDelay, Func<IResponseContext<TRequest, TResponse>, bool> shouldRetry)
        {
            return factoryResilienceSetter.UsePolly(
                new ResiliencePolicySettings<TRequest, TResponse>(maxRetries, getDelay, shouldRetry));
        }
        
        // TODO: doc
        public static INClientResilienceMethodSelector<TClient, TRequest, TResponse> UsePolly<TClient, TRequest, TResponse>(
            this INClientResilienceSetter<TClient, TRequest, TResponse> clientResilienceSetter, 
            IAsyncPolicy<IResponseContext<TRequest, TResponse>> asyncPolicy)
        {
            return clientResilienceSetter.Use(new PollyResiliencePolicyProvider<TRequest, TResponse>(asyncPolicy));
        }

        public static INClientFactoryResilienceMethodSelector<TRequest, TResponse> UsePolly<TRequest, TResponse>(
            this INClientFactoryResilienceSetter<TRequest, TResponse> factoryResilienceSetter, 
            IAsyncPolicy<IResponseContext<TRequest, TResponse>> asyncPolicy)
        {
            return factoryResilienceSetter.Use(new PollyResiliencePolicyProvider<TRequest, TResponse>(asyncPolicy));
        }
    }
}
