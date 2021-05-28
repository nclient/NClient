using System;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Serialization.System;

namespace NClient
{
    public class NClientFactory : INClientFactory
    {
        private readonly IHttpClientProvider? _httpClientProvider;
        private readonly ISerializerProvider? _serializerProvider;
        private readonly IResiliencePolicyProvider? _resiliencePolicyProvider;
        private readonly ILoggerFactory? _loggerFactory;

        public NClientFactory(
            IResiliencePolicyProvider? resiliencePolicyProvider = null,
            ILoggerFactory? loggerFactory = null)
        {
            _resiliencePolicyProvider = resiliencePolicyProvider;
            _loggerFactory = loggerFactory;
        }

        public NClientFactory(
            IHttpClientFactory httpClientFactory,
            string? httpClientName = null,
            IResiliencePolicyProvider? resiliencePolicyProvider = null,
            ILoggerFactory? loggerFactory = null)
            : this(httpClientFactory, httpClientName, jsonSerializerOptions: null,
                resiliencePolicyProvider, loggerFactory)
        {
        }

        public NClientFactory(
            JsonSerializerOptions jsonSerializerOptions,
            IResiliencePolicyProvider? resiliencePolicyProvider = null,
            ILoggerFactory? loggerFactory = null)
            : this(httpClientFactory: null, httpClientName: null, jsonSerializerOptions,
                resiliencePolicyProvider, loggerFactory)
        {
        }

        public NClientFactory(
            IHttpClientFactory? httpClientFactory = null,
            string? httpClientName = null,
            JsonSerializerOptions? jsonSerializerOptions = null,
            IResiliencePolicyProvider? resiliencePolicyProvider = null,
            ILoggerFactory? loggerFactory = null)
        {
            _httpClientProvider = httpClientFactory is not null
                ? new SystemHttpClientProvider(httpClientFactory, httpClientName)
                : null;
            _serializerProvider = jsonSerializerOptions is not null
                ? new SystemSerializerProvider(jsonSerializerOptions)
                : null;
            _resiliencePolicyProvider = resiliencePolicyProvider;
            _loggerFactory = loggerFactory;
        }

        internal NClientFactory(
            IHttpClientProvider? httpClientProvider = null,
            ISerializerProvider? serializerProvider = null,
            IResiliencePolicyProvider? resiliencePolicyProvider = null,
            ILoggerFactory? loggerFactory = null)
        {
            _serializerProvider = serializerProvider;
            _httpClientProvider = httpClientProvider;
            _resiliencePolicyProvider = resiliencePolicyProvider;
            _loggerFactory = loggerFactory;
        }

        public TInterface Create<TInterface>(string host)
            where TInterface : class
        {
            Ensure.IsNotNull(host, nameof(host));

            var nclientBuilder = new NClientBuilder()
                .Use<TInterface>(host);

            if (_httpClientProvider is not null)
                nclientBuilder = nclientBuilder.WithCustomHttpClient(_httpClientProvider);
            if (_serializerProvider is not null)
                nclientBuilder = nclientBuilder.WithCustomSerializer(_serializerProvider);
            if (_resiliencePolicyProvider is not null)
                nclientBuilder = nclientBuilder.WithResiliencePolicy(_resiliencePolicyProvider);
            if (_loggerFactory is not null)
                nclientBuilder = nclientBuilder.WithLogging(_loggerFactory.CreateLogger<TInterface>());

            return nclientBuilder.Build();
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Create<T> method.")]
        public TInterface Create<TInterface, TController>(string host)
            where TInterface : class
            where TController : TInterface
        {
            Ensure.IsNotNull(host, nameof(host));

            var nclientBuilder = new NClientBuilder()
                .Use<TInterface, TController>(host);

            if (_httpClientProvider is not null)
                nclientBuilder = nclientBuilder.WithCustomHttpClient(_httpClientProvider);
            if (_serializerProvider is not null)
                nclientBuilder = nclientBuilder.WithCustomSerializer(_serializerProvider);
            if (_resiliencePolicyProvider is not null)
                nclientBuilder = nclientBuilder.WithResiliencePolicy(_resiliencePolicyProvider);
            if (_loggerFactory is not null)
                nclientBuilder = nclientBuilder.WithLogging(_loggerFactory.CreateLogger<TInterface>());

            return nclientBuilder.Build();
        }
    }
}
