using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Resilience;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Serialization.System;

namespace NClient
{
    /// <summary>
    /// The factory used to create the client.
    /// </summary>
    public class NClientFactory : NClientStandaloneFactory
    {
        public NClientFactory(
            JsonSerializerOptions? jsonSerializerOptions = null,
            IResiliencePolicyProvider? resiliencePolicyProvider = null,
            ILoggerFactory? loggerFactory = null)
            : base(
                new SystemHttpClientProvider(),
                new SystemSerializerProvider(GetOrDefault(jsonSerializerOptions)),
                resiliencePolicyProvider,
                loggerFactory)
        {
        }

        public NClientFactory(
            IHttpClientFactory httpClientFactory,
            string? httpClientFactoryName = null,
            JsonSerializerOptions? jsonSerializerOptions = null,
            IResiliencePolicyProvider? resiliencePolicyProvider = null,
            ILoggerFactory? loggerFactory = null)
            : base(
                new SystemHttpClientProvider(httpClientFactory, httpClientFactoryName),
                new SystemSerializerProvider(GetOrDefault(jsonSerializerOptions)),
                resiliencePolicyProvider,
                loggerFactory)
        {
        }

        public NClientFactory(
            HttpClient httpClient,
            JsonSerializerOptions? jsonSerializerOptions = null,
            IResiliencePolicyProvider? resiliencePolicyProvider = null,
            ILoggerFactory? loggerFactory = null)
            : base(
                new SystemHttpClientProvider(httpClient),
                new SystemSerializerProvider(GetOrDefault(jsonSerializerOptions)),
                resiliencePolicyProvider,
                loggerFactory)
        {
        }

        private static JsonSerializerOptions GetOrDefault(JsonSerializerOptions? jsonSerializerOptions)
        {
            return jsonSerializerOptions ?? new JsonSerializerOptions();
        }
    }
}