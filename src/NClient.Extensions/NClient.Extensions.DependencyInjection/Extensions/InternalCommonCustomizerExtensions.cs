using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Customization;
using NClient.Providers.HttpClient.System;

namespace NClient.Extensions.DependencyInjection.Extensions
{
    internal static class InternalCommonCustomizerExtensions
    {
        public static TCustomizer WithRegisteredProviders<TCustomizer, TClient>(
            this INClientCommonCustomizer<TCustomizer, TClient, HttpRequestMessage, HttpResponseMessage> commonCustomizer,
            IServiceProvider serviceProvider, string? httpClientName)
            where TCustomizer : class, INClientCommonCustomizer<TCustomizer, TClient, HttpRequestMessage, HttpResponseMessage>
            where TClient : class
        {
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

            return commonCustomizer
                .TrySetSystemHttpClient(httpClientFactory, httpClientName)
                .TrySetLogging(loggerFactory);
        }

        private static TCustomizer TrySetSystemHttpClient<TCustomizer, TClient>(
            this INClientCommonCustomizer<TCustomizer, TClient, HttpRequestMessage, HttpResponseMessage> commonCustomizer,
            IHttpClientFactory? httpClientFactory, string? httpClientName = null)
            where TCustomizer : class, INClientCommonCustomizer<TCustomizer, TClient, HttpRequestMessage, HttpResponseMessage>
            where TClient : class
        {
            if (httpClientFactory is not null)
                return commonCustomizer.UsingSystemHttpClient(httpClientFactory, httpClientName);
            return (commonCustomizer as TCustomizer)!;
        }

        private static TBuilder TrySetLogging<TBuilder, TResult, TRequest, TResponse>(
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
