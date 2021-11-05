using NClient.Common.Helpers;
using NClient.Providers.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class FullResilienceExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithFullResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientAdvancedOptionalBuilder, 
            IResiliencePolicyProvider<TRequest, TResponse> provider) 
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));
            Ensure.IsNotNull(provider, nameof(provider));
            
            return clientAdvancedOptionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(provider));
        }

        public static INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithFullResilience<TRequest, TResponse>(
            this INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> clientAdvancedOptionalBuilder, 
            IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));
            Ensure.IsNotNull(provider, nameof(provider));
            
            return clientAdvancedOptionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(provider));
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithFullResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> clientOptionalBuilder, 
            IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            Ensure.IsNotNull(provider, nameof(provider));

            return WithFullResilience(clientOptionalBuilder.AsAdvanced(), provider).AsBasic();
        }
    }
}
