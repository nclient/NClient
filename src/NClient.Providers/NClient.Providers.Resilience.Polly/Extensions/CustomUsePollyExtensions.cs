using NClient.Providers.Resilience.Polly;
using NClient.Providers.Transport;
using Polly;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class CustomUsePollyExtensions
    {
        /// <summary>Sets a custom Polly resilience policy for specific method/methods.</summary>
        /// <param name="clientResilienceSetter"></param>
        /// <param name="asyncPolicy">The asynchronous policy defining all executions available.</param>
        public static INClientResilienceMethodSelector<TClient, TRequest, TResponse> UsePolly<TClient, TRequest, TResponse>(
            this INClientResilienceSetter<TClient, TRequest, TResponse> clientResilienceSetter, 
            IAsyncPolicy<IResponseContext<TRequest, TResponse>> asyncPolicy)
        {
            return clientResilienceSetter.Use(new CustomPollyResiliencePolicyProvider<TRequest, TResponse>(asyncPolicy));
        }

        /// <summary>Sets a custom Polly resilience policy for specific method/methods.</summary>
        /// <param name="factoryResilienceSetter"></param>
        /// <param name="asyncPolicy">The asynchronous policy defining all executions available.</param>
        public static INClientFactoryResilienceMethodSelector<TRequest, TResponse> UsePolly<TRequest, TResponse>(
            this INClientFactoryResilienceSetter<TRequest, TResponse> factoryResilienceSetter, 
            IAsyncPolicy<IResponseContext<TRequest, TResponse>> asyncPolicy)
        {
            return factoryResilienceSetter.Use(new CustomPollyResiliencePolicyProvider<TRequest, TResponse>(asyncPolicy));
        }
    }
}
