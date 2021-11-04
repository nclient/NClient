using NClient.Common.Helpers;
using NClient.Core.Extensions;
using NClient.Providers.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class SafeResilienceExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithSafeResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            IResiliencePolicyProvider<TRequest, TResponse> safeMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider) 
            where TClient : class
        {
            Ensure.IsNotNull(safeMethodProvider, nameof(safeMethodProvider));
            Ensure.IsNotNull(otherMethodProvider, nameof(otherMethodProvider));
            
            return clientOptionalBuilder.AsAdvanced()
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(otherMethodProvider)
                    .ForMethodsThat((_, request) => request.Type.IsSafe())
                    .Use(safeMethodProvider))
                .AsBasic();
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithSafeResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            IResiliencePolicyProvider<TRequest, TResponse> safeMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
        {
            Ensure.IsNotNull(safeMethodProvider, nameof(safeMethodProvider));
            Ensure.IsNotNull(otherMethodProvider, nameof(otherMethodProvider));
            
            return factoryOptionalBuilder.WithCustomResilience(x => x
                .ForAllMethods()
                .Use(otherMethodProvider)
                .ForMethodsThat((_, request) => request.Type.IsSafe())
                .Use(safeMethodProvider));
        }
    }
}
