using Microsoft.Extensions.Logging;
using NClient.Abstractions.Customization;

namespace NClient.Extensions
{
    internal static class InternalCommonCustomizerExtensions
    {
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
