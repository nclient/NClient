using NClient.Common.Helpers;
using NClient.Core.Extensions;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Transport;
using Polly;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class CustomPollyIdempotentResilienceExtensions
    {
        /// <summary>Sets a custom Polly resilience policy for idempotent methods (all except Create/Post).</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="idempotentMethodPolicy">The asynchronous policy defining all executions available for idempotent methods (all except Create/Post).</param>
        /// <param name="otherMethodPolicy">The asynchronous policy defining all executions available for non-idempotent methods (Create/Post).</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithPollyIdempotentResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            IAsyncPolicy<IResponseContext<TRequest, TResponse>> idempotentMethodPolicy, IAsyncPolicy<IResponseContext<TRequest, TResponse>> otherMethodPolicy)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(idempotentMethodPolicy, nameof(idempotentMethodPolicy));
            Ensure.IsNotNull(otherMethodPolicy, nameof(otherMethodPolicy));
            
            return optionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(new CustomPollyResiliencePolicyProvider<TRequest, TResponse>(otherMethodPolicy))
                    .ForMethodsThat((_, request) => request.Type.IsIdempotent())
                    .Use(new CustomPollyResiliencePolicyProvider<TRequest, TResponse>(idempotentMethodPolicy)));
        }

        /// <summary>Sets a custom Polly resilience policy for idempotent methods (all except Create/Post).</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="idempotentMethodPolicy">The asynchronous policy defining all executions available for idempotent methods (all except Create/Post).</param>
        /// <param name="otherMethodPolicy">The asynchronous policy defining all executions available for non-idempotent methods (Create/Post).</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithPollyIdempotentResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            IAsyncPolicy<IResponseContext<TRequest, TResponse>> idempotentMethodPolicy, IAsyncPolicy<IResponseContext<TRequest, TResponse>> otherMethodPolicy)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(idempotentMethodPolicy, nameof(idempotentMethodPolicy));
            Ensure.IsNotNull(otherMethodPolicy, nameof(otherMethodPolicy));
            
            return optionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(new CustomPollyResiliencePolicyProvider<TRequest, TResponse>(otherMethodPolicy))
                    .ForMethodsThat((_, request) => request.Type.IsIdempotent())
                    .Use(new CustomPollyResiliencePolicyProvider<TRequest, TResponse>(idempotentMethodPolicy)));
        }
    }
}
