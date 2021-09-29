using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.Resilience;

namespace NClient.Extensions
{
    internal static class InternalCommonCustomizerExtensions
    {
        public static TBuilder TrySetResiliencePolicy<TBuilder, TResult, TRequest, TResponse>(
            this INClientCommonCustomizer<TBuilder, TResult, TRequest, TResponse> clientBuilder,
            IMethodResiliencePolicyProvider<TRequest, TResponse>? methodResiliencePolicyProvider)
            where TBuilder : class, INClientCommonCustomizer<TBuilder, TResult, TRequest, TResponse>
        {
            if (methodResiliencePolicyProvider is not null)
                return clientBuilder.WithResiliencePolicy(methodResiliencePolicyProvider);
            return (clientBuilder as TBuilder)!;
        }

        public static TBuilder TrySetLogging<TBuilder, TResult, TRequest, TResponse>(
            this INClientCommonCustomizer<TBuilder, TResult, TRequest, TResponse> clientBuilder,
            ILoggerFactory? loggerFactory)
            where TBuilder : class, INClientCommonCustomizer<TBuilder, TResult, TRequest, TResponse>
        {
            if (loggerFactory is not null)
                return clientBuilder.WithLogging(loggerFactory);
            return (clientBuilder as TBuilder)!;
        }
    }
}
