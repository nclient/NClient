using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;

namespace NClient.Extensions.DependencyInjection.Extensions
{
    internal static class InternalCommonCustomizerExtensions
    {
        public static TCustomizer WithRegisteredProviders<TCustomizer, TInterface>(
            this INClientCommonCustomizer<TCustomizer, TInterface, HttpRequestMessage, HttpResponseMessage> commonCustomizer,
            IServiceProvider serviceProvider, string? httpClientName)
            where TCustomizer : class, INClientCommonCustomizer<TCustomizer, TInterface, HttpRequestMessage, HttpResponseMessage>
            where TInterface : class
        {
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

            return commonCustomizer
                .TrySetCustomHttpClient(httpClientFactory, httpClientName)
                .TrySetLogging(loggerFactory);
        }
    }
}
