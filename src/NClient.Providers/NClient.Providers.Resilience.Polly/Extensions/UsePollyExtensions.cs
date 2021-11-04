using NClient.Providers.Resilience.Polly;
using NClient.Providers.Transport;
using Polly;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UsePollyExtensions
    {
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
