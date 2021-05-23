using System.Net.Http;
using System.Text.Json;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Serialization.System;
using Polly;

namespace NClient.Extensions
{
    public static class NClientFactoryBuilderExtensions
    {
        public static INClientFactoryBuilder WithCustomHttpClient(
            this INClientFactoryBuilder clientFactoryBuilder,
            IHttpClientFactory httpClientFactory, string? httpClientName = null)
        {
            return clientFactoryBuilder.WithCustomHttpClient(new SystemHttpClientProvider(httpClientFactory, httpClientName));
        }

        public static INClientFactoryBuilder WithCustomSerializer(
            this INClientFactoryBuilder clientFactoryBuilder,
            JsonSerializerOptions jsonSerializerOptions)
        {
            return clientFactoryBuilder.WithCustomSerializer(new SystemSerializerProvider(jsonSerializerOptions));
        }

        public static INClientFactoryBuilder WithResiliencePolicy(
            this INClientFactoryBuilder clientFactoryBuilder,
            IAsyncPolicy<HttpResponse> asyncPolicy)
        {
            return clientFactoryBuilder.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }
    }
}