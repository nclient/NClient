using System.Net.Http;
using NClient.Abstractions;
using NClient.Providers.HttpClient.System;

namespace NClient
{
    internal static class InternalOptionalBuilderBaseExtensions
    {
        public static TBuilder TrySetCustomHttpClient<TBuilder, TInterface>(
            this IOptionalBuilderBase<TBuilder, TInterface> clientBuilder,
            IHttpClientFactory? httpClientFactory, string? httpClientName = null)
            where TBuilder : class, IOptionalBuilderBase<TBuilder, TInterface>
            where TInterface : class
        {
            if (httpClientFactory is not null)
                return clientBuilder.WithCustomHttpClient(new SystemHttpClientProvider(httpClientFactory, httpClientName));
            return (clientBuilder as TBuilder)!;
        }
    }
}
