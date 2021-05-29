using System.Net.Http;
using System.Text.Json;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Common.Helpers;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Serialization.System;
using Polly;

namespace NClient.Extensions
{
    public static class NClientFactoryBuilderExtensions
    {
        public static IOptionalNClientFactoryBuilder WithCustomHttpClient(
            this IOptionalNClientFactoryBuilder optionalNClientFactoryBuilder,
            IHttpClientFactory httpClientFactory, string? httpClientName = null)
        {
            Ensure.IsNotNull(optionalNClientFactoryBuilder, nameof(optionalNClientFactoryBuilder));
            Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));

            return optionalNClientFactoryBuilder.WithCustomHttpClient(new SystemHttpClientProvider(httpClientFactory, httpClientName));
        }

        public static IOptionalNClientFactoryBuilder WithCustomSerializer(
            this IOptionalNClientFactoryBuilder optionalNClientFactoryBuilder,
            JsonSerializerOptions jsonSerializerOptions)
        {
            Ensure.IsNotNull(optionalNClientFactoryBuilder, nameof(optionalNClientFactoryBuilder));
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            return optionalNClientFactoryBuilder.WithCustomSerializer(new SystemSerializerProvider(jsonSerializerOptions));
        }

        public static IOptionalNClientFactoryBuilder WithResiliencePolicy(
            this IOptionalNClientFactoryBuilder optionalNClientFactoryBuilder,
            IAsyncPolicy<HttpResponse> asyncPolicy)
        {
            Ensure.IsNotNull(optionalNClientFactoryBuilder, nameof(optionalNClientFactoryBuilder));
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));

            return optionalNClientFactoryBuilder.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }
    }
}