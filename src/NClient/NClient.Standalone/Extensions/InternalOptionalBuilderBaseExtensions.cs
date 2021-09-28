using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.Resilience;

namespace NClient.Extensions
{
    internal static class InternalOptionalBuilderBaseExtensions
    {
        public static TBuilder TrySetResiliencePolicy<TBuilder, TResult>(
            this INClientCommonCustomizer<TBuilder, TResult> clientBuilder,
            IMethodResiliencePolicyProvider? methodResiliencePolicyProvider)
            where TBuilder : class, INClientCommonCustomizer<TBuilder, TResult>
        {
            if (methodResiliencePolicyProvider is not null)
                return clientBuilder.WithResiliencePolicy(methodResiliencePolicyProvider);
            return (clientBuilder as TBuilder)!;
        }

        public static TBuilder TrySetLogging<TBuilder, TResult>(
            this INClientCommonCustomizer<TBuilder, TResult> clientBuilder,
            ILoggerFactory? loggerFactory)
            where TBuilder : class, INClientCommonCustomizer<TBuilder, TResult>
        {
            if (loggerFactory is not null)
                return clientBuilder.WithLogging(loggerFactory);
            return (clientBuilder as TBuilder)!;
        }
    }
}
