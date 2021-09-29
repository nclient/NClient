using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Resilience;
using NClient.Core.Resilience;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Serialization.System;
using Polly;

namespace NClient
{
    /// <summary>
    /// The factory used to create the client.
    /// </summary>
    public class NClientFactory : NClientStandaloneFactory<HttpRequestMessage, HttpResponseMessage>
    {
        public NClientFactory(
            JsonSerializerOptions? jsonSerializerOptions = null,
            IAsyncPolicy<ResponseContext<HttpResponseMessage>>? resiliencePolicy = null,
            ILoggerFactory? loggerFactory = null)
            : base(
                new SystemHttpClientProvider(),
                new SystemHttpMessageBuilderProvider(),
                new SystemHttpClientExceptionFactory(),
                serializerProvider: GetOrDefault(jsonSerializerOptions),
                methodResiliencePolicyProvider: GetOrDefault(resiliencePolicy),
                loggerFactory)
        {
        }

        public NClientFactory(
            IHttpClientFactory httpClientFactory,
            string? httpClientName = null,
            JsonSerializerOptions? jsonSerializerOptions = null,
            IAsyncPolicy<ResponseContext<HttpResponseMessage>>? resiliencePolicy = null,
            ILoggerFactory? loggerFactory = null)
            : base(
                new SystemHttpClientProvider(httpClientFactory, httpClientName),
                new SystemHttpMessageBuilderProvider(),
                new SystemHttpClientExceptionFactory(),
                serializerProvider: GetOrDefault(jsonSerializerOptions),
                methodResiliencePolicyProvider: GetOrDefault(resiliencePolicy),
                loggerFactory)
        {
        }

        public NClientFactory(
            HttpClient httpClient,
            JsonSerializerOptions? jsonSerializerOptions = null,
            IAsyncPolicy<ResponseContext<HttpResponseMessage>>? resiliencePolicy = null,
            ILoggerFactory? loggerFactory = null)
            : base(
                new SystemHttpClientProvider(httpClient),
                new SystemHttpMessageBuilderProvider(),
                new SystemHttpClientExceptionFactory(),
                serializerProvider: GetOrDefault(jsonSerializerOptions),
                methodResiliencePolicyProvider: GetOrDefault(resiliencePolicy),
                loggerFactory)
        {
        }

        private static SystemSerializerProvider GetOrDefault(JsonSerializerOptions? jsonSerializerOptions)
        {
            return jsonSerializerOptions is not null
                ? new SystemSerializerProvider(jsonSerializerOptions)
                : new SystemSerializerProvider();
        }

        private static DefaultMethodResiliencePolicyProvider<HttpResponseMessage>? GetOrDefault(IAsyncPolicy<ResponseContext<HttpResponseMessage>>? resiliencePolicy)
        {
            return resiliencePolicy is not null
                ? new DefaultMethodResiliencePolicyProvider<HttpResponseMessage>(new PollyResiliencePolicyProvider<HttpResponseMessage>(resiliencePolicy))
                : null;
        }
    }
}
