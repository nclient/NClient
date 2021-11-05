using NClient.Common.Helpers;
using NClient.Core.Extensions;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Transport;
using Polly;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class PollyIdempotentResilienceExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithPollyIdempotentResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientAdvancedOptionalBuilder,
            IAsyncPolicy<IResponseContext<TRequest, TResponse>> idempotentMethodPolicy, IAsyncPolicy<IResponseContext<TRequest, TResponse>> otherMethodPolicy)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));
            Ensure.IsNotNull(idempotentMethodPolicy, nameof(idempotentMethodPolicy));
            Ensure.IsNotNull(otherMethodPolicy, nameof(otherMethodPolicy));
            
            return clientAdvancedOptionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(new PollyResiliencePolicyProvider<TRequest, TResponse>(otherMethodPolicy))
                    .ForMethodsThat((_, request) => request.Type.IsIdempotent())
                    .Use(new PollyResiliencePolicyProvider<TRequest, TResponse>(idempotentMethodPolicy)));
        }

        public static INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithPollyIdempotentResilience<TRequest, TResponse>(
            this INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> clientAdvancedOptionalBuilder,
            IAsyncPolicy<IResponseContext<TRequest, TResponse>> idempotentMethodPolicy, IAsyncPolicy<IResponseContext<TRequest, TResponse>> otherMethodPolicy)
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));
            Ensure.IsNotNull(idempotentMethodPolicy, nameof(idempotentMethodPolicy));
            Ensure.IsNotNull(otherMethodPolicy, nameof(otherMethodPolicy));
            
            return clientAdvancedOptionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(new PollyResiliencePolicyProvider<TRequest, TResponse>(otherMethodPolicy))
                    .ForMethodsThat((_, request) => request.Type.IsIdempotent())
                    .Use(new PollyResiliencePolicyProvider<TRequest, TResponse>(idempotentMethodPolicy)));
        }
        
        /// <summary>
        /// Sets resilience policy provider for idempotent HTTP methods (all except POST).
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        /// <param name="idempotentMethodPolicy">The settings for resilience policy provider for idempotent methods.</param>
        /// <param name="otherMethodPolicy">The settings for resilience policy provider for other methods.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithPollyIdempotentResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> clientOptionalBuilder,
            IAsyncPolicy<IResponseContext<TRequest, TResponse>> idempotentMethodPolicy, IAsyncPolicy<IResponseContext<TRequest, TResponse>> otherMethodPolicy)
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            Ensure.IsNotNull(idempotentMethodPolicy, nameof(idempotentMethodPolicy));
            Ensure.IsNotNull(otherMethodPolicy, nameof(otherMethodPolicy));
            
            return WithPollyIdempotentResilience(clientOptionalBuilder.AsAdvanced(), idempotentMethodPolicy, otherMethodPolicy).AsBasic();
        }
    }
}
