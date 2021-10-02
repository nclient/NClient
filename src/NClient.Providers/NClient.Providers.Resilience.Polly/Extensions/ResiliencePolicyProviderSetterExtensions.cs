using NClient.Abstractions.Customization.Resilience;
using NClient.Abstractions.Resilience;
using Polly;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Resilience.Polly
{
    public static class ResiliencePolicyProviderSetterExtensions
    {
        // TODO: doc
        public static IResiliencePolicyMethodSelector<TClient, TRequest, TResponse> UsePolly<TClient, TRequest, TResponse>(
            this IResiliencePolicyProviderSetter<TClient, TRequest, TResponse> resiliencePolicyProviderSetter, 
            IAsyncPolicy<ResponseContext<TRequest, TResponse>> asyncPolicy)
        {
            return resiliencePolicyProviderSetter.Use(new PollyResiliencePolicyProvider<TRequest, TResponse>(asyncPolicy));
        }
        
        public static IResiliencePolicyMethodSelector<TClient, TRequest, TResponse> UsePolly<TClient, TRequest, TResponse>(
            this IResiliencePolicyProviderSetter<TClient, TRequest, TResponse> resiliencePolicyProviderSetter, 
            IResiliencePolicySettings<TRequest, TResponse> policySettings)
        {
            return resiliencePolicyProviderSetter.Use(new ConfiguredPollyResiliencePolicyProvider<TRequest, TResponse>(policySettings));
        }
        
        public static IResiliencePolicyMethodSelector<TRequest, TResponse> UsePolly<TRequest, TResponse>(
            this IResiliencePolicyProviderSetter<TRequest, TResponse> resiliencePolicyProviderSetter, 
            IAsyncPolicy<ResponseContext<TRequest, TResponse>> asyncPolicy)
        {
            return resiliencePolicyProviderSetter.Use(new PollyResiliencePolicyProvider<TRequest, TResponse>(asyncPolicy));
        }
        
        public static IResiliencePolicyMethodSelector<TRequest, TResponse> UsePolly<TRequest, TResponse>(
            this IResiliencePolicyProviderSetter<TRequest, TResponse> resiliencePolicyProviderSetter, 
            IResiliencePolicySettings<TRequest, TResponse> policySettings)
        {
            return resiliencePolicyProviderSetter.Use(new ConfiguredPollyResiliencePolicyProvider<TRequest, TResponse>(policySettings));
        }
    }
}
