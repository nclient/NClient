using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;

namespace NClient.Extensions.DependencyInjection.Extensions
{
    internal static class InternalOptionalBuilderBaseExtensions
    {
        public static TBuilder WithRegisteredProviders<TBuilder, TInterface>(
            this IOptionalBuilderBase<TBuilder, TInterface> optionalNClientBuilder,
            IServiceProvider serviceProvider, string? httpClientName)
            where TBuilder : class, IOptionalBuilderBase<TBuilder, TInterface>
            where TInterface : class
        {
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            
            return optionalNClientBuilder
                .TrySetCustomHttpClient(httpClientFactory, httpClientName)
                .TrySetLogging(loggerFactory);
        }
    }
}