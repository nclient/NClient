using NClient.Common.Helpers;
using NClient.Core.Extensions;
using NClient.Providers.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class IdempotentResilienceExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithIdempotentResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            IResiliencePolicyProvider<TRequest, TResponse> idempotentMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(idempotentMethodProvider, nameof(idempotentMethodProvider));
            Ensure.IsNotNull(otherMethodProvider, nameof(otherMethodProvider));
            
            return optionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(otherMethodProvider)
                    .ForMethodsThat((_, request) => request.Type.IsIdempotent())
                    .Use(idempotentMethodProvider));
        }

        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithIdempotentResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            IResiliencePolicyProvider<TRequest, TResponse> idempotentMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(idempotentMethodProvider, nameof(idempotentMethodProvider));
            Ensure.IsNotNull(otherMethodProvider, nameof(otherMethodProvider));
            
            return optionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(otherMethodProvider)
                    .ForMethodsThat((_, request) => request.Type.IsIdempotent())
                    .Use(idempotentMethodProvider));
        }
    }
}
