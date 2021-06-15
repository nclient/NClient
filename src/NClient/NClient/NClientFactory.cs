﻿using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.HttpClients;
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
            IAsyncPolicy<HttpResponse>? resiliencePolicy = null,
            ILoggerFactory? loggerFactory = null)
            : base(
                new SystemHttpClientProvider(),
                serializerProvider: GetOrDefault(jsonSerializerOptions),
                resiliencePolicyProvider: GetOrDefault(resiliencePolicy),
                loggerFactory)
        {
        }

        public NClientFactory(
            IHttpClientFactory httpClientFactory,
            string? httpClientFactoryName = null,
            JsonSerializerOptions? jsonSerializerOptions = null,
            IAsyncPolicy<HttpResponse>? resiliencePolicy = null,
            ILoggerFactory? loggerFactory = null)
            : base(
                new SystemHttpClientProvider(httpClientFactory, httpClientFactoryName),
                serializerProvider: GetOrDefault(jsonSerializerOptions),
                resiliencePolicyProvider: GetOrDefault(resiliencePolicy),
                loggerFactory)
        {
        }

        public NClientFactory(
            HttpClient httpClient,
            JsonSerializerOptions? jsonSerializerOptions = null,
            IAsyncPolicy<HttpResponse>? resiliencePolicy = null,
            ILoggerFactory? loggerFactory = null)
            : base(
                new SystemHttpClientProvider(httpClient),
                serializerProvider: GetOrDefault(jsonSerializerOptions),
                resiliencePolicyProvider: GetOrDefault(resiliencePolicy),
                loggerFactory)
        {
        }

        private static SystemSerializerProvider GetOrDefault(JsonSerializerOptions? jsonSerializerOptions)
        {
            return jsonSerializerOptions is not null
                ? new SystemSerializerProvider(jsonSerializerOptions)
                : new SystemSerializerProvider();
        }

        private static PollyResiliencePolicyProvider? GetOrDefault(IAsyncPolicy<HttpResponse>? resiliencePolicy)
        {
            return resiliencePolicy is not null
                ? new PollyResiliencePolicyProvider(resiliencePolicy)
                : null;
        }
    }
}