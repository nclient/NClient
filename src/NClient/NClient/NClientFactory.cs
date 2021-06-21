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
    public class NClientFactory : NClientStandaloneFactory
    {
        public NClientFactory(
            JsonSerializerOptions? jsonSerializerOptions = null,
            IAsyncPolicy<ResponseContext>? resiliencePolicy = null,
            ILoggerFactory? loggerFactory = null)
            : base(
                new SystemHttpClientProvider(),
                serializerProvider: GetOrDefault(jsonSerializerOptions),
                methodResiliencePolicyProvider: GetOrDefault(resiliencePolicy),
                loggerFactory)
        {
        }

        public NClientFactory(
            IHttpClientFactory httpClientFactory,
            string? httpClientFactoryName = null,
            JsonSerializerOptions? jsonSerializerOptions = null,
            IAsyncPolicy<ResponseContext>? resiliencePolicy = null,
            ILoggerFactory? loggerFactory = null)
            : base(
                new SystemHttpClientProvider(httpClientFactory, httpClientFactoryName),
                serializerProvider: GetOrDefault(jsonSerializerOptions),
                methodResiliencePolicyProvider: GetOrDefault(resiliencePolicy),
                loggerFactory)
        {
        }

        public NClientFactory(
            HttpClient httpClient,
            JsonSerializerOptions? jsonSerializerOptions = null,
            IAsyncPolicy<ResponseContext>? resiliencePolicy = null,
            ILoggerFactory? loggerFactory = null)
            : base(
                new SystemHttpClientProvider(httpClient),
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

        private static DefaultMethodResiliencePolicyProvider? GetOrDefault(IAsyncPolicy<ResponseContext>? resiliencePolicy)
        {
            return resiliencePolicy is not null
                ? new DefaultMethodResiliencePolicyProvider(new PollyResiliencePolicyProvider(resiliencePolicy))
                : null;
        }
    }
}