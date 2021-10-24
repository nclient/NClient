using NClient.Common.Helpers;
using NClient.Core.Extensions;
using NClient.Providers.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class IdempotentResilienceExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithIdempotentResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            IResiliencePolicyProvider<TRequest, TResponse> idempotentMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
            where TClient : class
        {
            Ensure.IsNotNull(idempotentMethodProvider, nameof(idempotentMethodProvider));
            Ensure.IsNotNull(otherMethodProvider, nameof(otherMethodProvider));
            
            return clientOptionalBuilder.WithCustomResilience(x => x
                .ForAllMethods()
                .Use(otherMethodProvider)
                .ForMethodsThat((_, request) => request.Type.IsIdempotent())
                .Use(idempotentMethodProvider));
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithIdempotentResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            IResiliencePolicyProvider<TRequest, TResponse> idempotentMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
        {
            Ensure.IsNotNull(idempotentMethodProvider, nameof(idempotentMethodProvider));
            Ensure.IsNotNull(otherMethodProvider, nameof(otherMethodProvider));
            
            return factoryOptionalBuilder.WithCustomResilience(x => x
                .ForAllMethods()
                .Use(otherMethodProvider)
                .ForMethodsThat((_, request) => request.Type.IsIdempotent())
                .Use(idempotentMethodProvider));
        }
    }
}
