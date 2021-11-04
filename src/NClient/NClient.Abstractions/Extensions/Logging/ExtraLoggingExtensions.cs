using System.Linq;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class LoggingExtensions
    {
        /// <summary>
        /// Sets instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="logger">The logger for a client.</param>
        // Not advanced
        public static INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithLogging<TClient, TRequest, TResponse>(
            this INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            ILogger logger, params ILogger[] extraLoggers)
            where TClient : class
        {
            return clientOptionalBuilder.WithLogging(extraLoggers.Concat(new[] { logger }));
        }

        /// <summary>
        /// Sets instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="logger">The logger for a client.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithLogging<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder, 
            ILogger logger, params ILogger[] extraLoggers) 
            where TClient : class
        {
            return clientOptionalBuilder.WithLogging(extraLoggers.Concat(new[] { logger }));
        }
    }
}
