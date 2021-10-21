using NClient.Abstractions.Building;
using NClient.Abstractions.Providers.Resilience;
using NClient.Common.Helpers;
using NClient.Core.Extensions;

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
            
            return clientOptionalBuilder.WithCustomResilience(x => x
                .ForAllMethods()
                .Use(otherMethodProvider)
                .ForMethodsThat((_, httpRequest) => httpRequest.Method.IsSafeMethod())
                .Use(safeMethodProvider));
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
                .ForMethodsThat((_, httpRequest) => httpRequest.Method.IsSafeMethod())
                .Use(safeMethodProvider));
        }
    }
}
