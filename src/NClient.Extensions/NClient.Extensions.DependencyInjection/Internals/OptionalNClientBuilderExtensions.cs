using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;

namespace NClient.Extensions.DependencyInjection.Internals
{
    internal static class OptionalNClientBuilderExtensions
    {
        public static IOptionalNClientBuilder<TInterface> WithRegisteredProviders<TInterface>(
            this IOptionalNClientBuilder<TInterface> optionalNClientBuilder, 
            IServiceProvider serviceProvider, string? httpClientName)
            where TInterface : class
        {
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            if (httpClientFactory is not null)
                optionalNClientBuilder.WithCustomHttpClient(httpClientFactory, httpClientName);

            var logger = serviceProvider.GetService<ILogger<TInterface>>();
            if (logger is not null)
                optionalNClientBuilder.WithLogging(logger);

            return optionalNClientBuilder;
        }
    }
}