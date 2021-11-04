using NClient.Common.Helpers;
using NClient.Core.Extensions;
using NClient.Providers.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class IdempotentResilienceExtensions
    {
        public static INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithIdempotentResilience<TClient, TRequest, TResponse>(
            this INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> clientAdvancedOptionalBuilder,
            IResiliencePolicyProvider<TRequest, TResponse> idempotentMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));
            Ensure.IsNotNull(idempotentMethodProvider, nameof(idempotentMethodProvider));
            Ensure.IsNotNull(otherMethodProvider, nameof(otherMethodProvider));
            
            return clientAdvancedOptionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(otherMethodProvider)
                    .ForMethodsThat((_, request) => request.Type.IsIdempotent())
                    .Use(idempotentMethodProvider));
        }
        
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithIdempotentResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            IResiliencePolicyProvider<TRequest, TResponse> idempotentMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            Ensure.IsNotNull(idempotentMethodProvider, nameof(idempotentMethodProvider));
            Ensure.IsNotNull(otherMethodProvider, nameof(otherMethodProvider));
            
            return WithIdempotentResilience(clientOptionalBuilder.AsAdvanced(), idempotentMethodProvider, otherMethodProvider).AsBasic();
        }
        
        public static INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithIdempotentResilience<TRequest, TResponse>(
            this INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> clientAdvancedOptionalBuilder,
            IResiliencePolicyProvider<TRequest, TResponse> idempotentMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));
            Ensure.IsNotNull(idempotentMethodProvider, nameof(idempotentMethodProvider));
            Ensure.IsNotNull(otherMethodProvider, nameof(otherMethodProvider));
            
            return clientAdvancedOptionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(otherMethodProvider)
                    .ForMethodsThat((_, request) => request.Type.IsIdempotent())
                    .Use(idempotentMethodProvider));
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithIdempotentResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> clientOptionalBuilder,
            IResiliencePolicyProvider<TRequest, TResponse> idempotentMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            Ensure.IsNotNull(idempotentMethodProvider, nameof(idempotentMethodProvider));
            Ensure.IsNotNull(otherMethodProvider, nameof(otherMethodProvider));
            
            return WithIdempotentResilience(clientOptionalBuilder.AsAdvanced(), idempotentMethodProvider, otherMethodProvider).AsBasic();
        }
    }
}
