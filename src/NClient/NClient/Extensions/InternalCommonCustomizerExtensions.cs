using System.Net.Http;
using NClient.Abstractions;
using NClient.Providers.HttpClient.System;

namespace NClient.Extensions
{
    internal static class InternalCommonCustomizerExtensions
    {
        public static TCustomizer TrySetCustomHttpClient<TCustomizer, TInterface>(
            this INClientCommonCustomizer<TCustomizer, TInterface, HttpRequestMessage, HttpResponseMessage> commonCustomizer,
            IHttpClientFactory? httpClientFactory, string? httpClientName = null)
            where TCustomizer : class, INClientCommonCustomizer<TCustomizer, TInterface, HttpRequestMessage, HttpResponseMessage>
            where TInterface : class
        {
            if (httpClientFactory is not null)
                return commonCustomizer.WithCustomHttpClient(
                    new SystemHttpClientProvider(httpClientFactory, httpClientName), 
                    new SystemHttpMessageBuilderProvider(),
                    new SystemHttpClientExceptionFactory());
            return (commonCustomizer as TCustomizer)!;
        }
    }
}
