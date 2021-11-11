using NClient.Common.Helpers;
using NClient.Providers.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class FullResilienceExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithFullResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder, 
            IResiliencePolicyProvider<TRequest, TResponse> provider) 
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(provider, nameof(provider));
            
            return optionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(provider));
        }

        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithFullResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder, 
            IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(provider, nameof(provider));
            
            return optionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(provider));
        }
    }
}
