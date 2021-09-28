using System.Net.Http;
using NClient.Abstractions;
using NClient.Providers.HttpClient.System;

namespace NClient.Extensions
{
    internal static class InternalCommonCustomizerExtensions
    {
        public static TCustomizer TrySetCustomHttpClient<TCustomizer, TInterface>(
            this INClientCommonCustomizer<TCustomizer, TInterface> commonCustomizer,
            IHttpClientFactory? httpClientFactory, string? httpClientName = null)
            where TCustomizer : class, INClientCommonCustomizer<TCustomizer, TInterface>
            where TInterface : class
        {
            if (httpClientFactory is not null)
                return commonCustomizer.WithCustomHttpClient(new SystemHttpClientProvider(httpClientFactory, httpClientName));
            return (commonCustomizer as TCustomizer)!;
        }
    }
}
