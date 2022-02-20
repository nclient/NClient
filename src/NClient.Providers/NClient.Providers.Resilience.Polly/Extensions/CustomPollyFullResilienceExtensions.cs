using NClient.Common.Helpers;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Transport;
using Polly;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class CustomPollyFullResilienceExtensions
    {
        /// <summary>Sets a custom Polly resilience policy for all types of methods.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="asyncPolicy">The asynchronous policy defining all executions available.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithPollyFullResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            IAsyncPolicy<IResponseContext<TRequest, TResponse>> asyncPolicy)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));
            
            return optionalBuilder.WithResilience(x => x
                .ForAllMethods()
                .Use(new CustomPollyResiliencePolicyProvider<TRequest, TResponse>(asyncPolicy)));
        }

        /// <summary>Sets a custom Polly resilience policy for all types of methods.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="asyncPolicy">The asynchronous policy defining all executions available.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithPollyFullResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            IAsyncPolicy<IResponseContext<TRequest, TResponse>> asyncPolicy)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));
            
            return optionalBuilder.WithResilience(x => x
                .ForAllMethods()
                .Use(new CustomPollyResiliencePolicyProvider<TRequest, TResponse>(asyncPolicy)));
        }
    }
}
