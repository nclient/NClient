using NClient.Common.Helpers;
using NClient.Core.Extensions;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Transport;
using Polly;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class CustomPollySafeResilienceExtensions
    {
        /// <summary>Sets a custom Polly resilience policy for safe methods (Info/Options, Check/Head, Read/Get).</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="safeMethodPolicy">The asynchronous policy defining all executions available for safe methods (Info/Options, Check/Head, Read/Get).</param>
        /// <param name="otherMethodPolicy">The asynchronous policy defining all executions available for unsafe methods (all except Info/Options, Check/Head, Read/Get).</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithPollySafeResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            IAsyncPolicy<IResponseContext<TRequest, TResponse>> safeMethodPolicy, IAsyncPolicy<IResponseContext<TRequest, TResponse>> otherMethodPolicy)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(safeMethodPolicy, nameof(safeMethodPolicy));
            Ensure.IsNotNull(otherMethodPolicy, nameof(otherMethodPolicy));
            
            return optionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(new CustomPollyResiliencePolicyProvider<TRequest, TResponse>(otherMethodPolicy))
                    .ForMethodsThat((_, request) => request.Type.IsSafe())
                    .Use(new CustomPollyResiliencePolicyProvider<TRequest, TResponse>(safeMethodPolicy)));
        }
        
        /// <summary>Sets a custom Polly resilience policy for safe methods (Info/Options, Check/Head, Read/Get).</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="safeMethodPolicy">The asynchronous policy defining all executions available for safe methods (Info/Options, Check/Head, Read/Get).</param>
        /// <param name="otherMethodPolicy">The asynchronous policy defining all executions available for unsafe methods (all except Info/Options, Check/Head, Read/Get).</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithPollySafeResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            IAsyncPolicy<IResponseContext<TRequest, TResponse>> safeMethodPolicy, IAsyncPolicy<IResponseContext<TRequest, TResponse>> otherMethodPolicy)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(safeMethodPolicy, nameof(safeMethodPolicy));
            Ensure.IsNotNull(otherMethodPolicy, nameof(otherMethodPolicy));
            
            return optionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(new CustomPollyResiliencePolicyProvider<TRequest, TResponse>(otherMethodPolicy))
                    .ForMethodsThat((_, request) => request.Type.IsSafe())
                    .Use(new CustomPollyResiliencePolicyProvider<TRequest, TResponse>(safeMethodPolicy)));
        }
    }
}
