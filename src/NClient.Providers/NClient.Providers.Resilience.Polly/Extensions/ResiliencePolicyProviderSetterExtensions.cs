using NClient.Abstractions.Customization.Resilience;
using NClient.Abstractions.Resilience;
using Polly;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Resilience.Polly
{
    public static class ResiliencePolicyProviderSetterExtensions
    {
        public static IResiliencePolicyMethodSelector<TInterface, TRequest, TResponse> UsePolly<TInterface, TRequest, TResponse>(
            this IResiliencePolicyProviderSetter<TInterface, TRequest, TResponse> resiliencePolicyProviderSetter, 
            IAsyncPolicy<ResponseContext<TRequest, TResponse>> asyncPolicy)
        {
            return resiliencePolicyProviderSetter.Use(new PollyResiliencePolicyProvider<TRequest, TResponse>(asyncPolicy));
        }
        
        public static IResiliencePolicyMethodSelector<TRequest, TResponse> UsePolly<TRequest, TResponse>(
            this IResiliencePolicyProviderSetter<TRequest, TResponse> resiliencePolicyProviderSetter, 
            IAsyncPolicy<ResponseContext<TRequest, TResponse>> asyncPolicy)
        {
            return resiliencePolicyProviderSetter.Use(new PollyResiliencePolicyProvider<TRequest, TResponse>(asyncPolicy));
        }
    }
}
