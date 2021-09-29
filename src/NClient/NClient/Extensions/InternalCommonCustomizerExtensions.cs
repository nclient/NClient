using System.Net.Http;
using NClient.Abstractions.Customization;
using NClient.Providers.HttpClient.System;

namespace NClient.Extensions
{
    internal static class InternalCommonCustomizerExtensions
    {
        public static TCustomizer TrySetSystemHttpClient<TCustomizer, TInterface>(
            this INClientCommonCustomizer<TCustomizer, TInterface, HttpRequestMessage, HttpResponseMessage> commonCustomizer,
            IHttpClientFactory? httpClientFactory, string? httpClientName = null)
            where TCustomizer : class, INClientCommonCustomizer<TCustomizer, TInterface, HttpRequestMessage, HttpResponseMessage>
            where TInterface : class
        {
            if (httpClientFactory is not null)
                return commonCustomizer.UsingSystemHttpClient(httpClientFactory, httpClientName);
            return (commonCustomizer as TCustomizer)!;
        }
    }
}
