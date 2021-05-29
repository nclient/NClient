using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;

namespace NClient.Extensions.DependencyInjection.Internals
{
    internal static class OptionalBuilderBaseExtensions
    {
        public static TBuilder WithRegisteredProviders<TBuilder, TInterface>(
            this IOptionalBuilderBase<TBuilder, TInterface> optionalNClientBuilder,
            IServiceProvider serviceProvider, string? httpClientName)
            where TBuilder : IOptionalBuilderBase<TBuilder, TInterface>
            where TInterface : class
        {
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            if (httpClientFactory is not null)
                optionalNClientBuilder.WithCustomHttpClient(httpClientFactory, httpClientName);

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            if (loggerFactory is not null)
                optionalNClientBuilder.WithLogging(loggerFactory);

            return (TBuilder)optionalNClientBuilder;
        }
    }
}