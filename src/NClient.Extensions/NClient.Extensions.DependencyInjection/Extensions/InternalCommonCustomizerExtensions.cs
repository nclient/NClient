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
        public static TCustomizer TrySetSystemHttpClient<TCustomizer>(
            this INClientCommonCustomizer<TCustomizer, HttpRequestMessage, HttpResponseMessage> commonCustomizer,
            IServiceProvider serviceProvider, string? httpClientName)
            where TCustomizer : class, INClientCommonCustomizer<TCustomizer, HttpRequestMessage, HttpResponseMessage>
        {
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            
            if (httpClientFactory is not null)
                return commonCustomizer.UsingSystemHttpClient(httpClientFactory, httpClientName);
            return (commonCustomizer as TCustomizer)!;
        }

        public static TBuilder TrySetLogging<TBuilder, TRequest, TResponse>(
            this INClientCommonCustomizer<TBuilder, TRequest, TResponse> clientBuilder,
            IServiceProvider serviceProvider)
            where TBuilder : class, INClientCommonCustomizer<TBuilder, TRequest, TResponse>
        {
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            
            if (loggerFactory is not null)
                return clientBuilder.WithLogging(loggerFactory);
            return (clientBuilder as TBuilder)!;
        }
    }
}
