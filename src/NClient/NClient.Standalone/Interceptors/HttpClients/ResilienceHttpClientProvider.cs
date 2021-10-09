﻿using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Resilience;

namespace NClient.Standalone.Interceptors.HttpClients
{
    internal interface IResilienceHttpClientProvider<TRequest, TResponse>
    {
        IResilienceHttpClient<TRequest, TResponse> Create(IResiliencePolicyProvider<TRequest, TResponse>? resiliencePolicyProvider);
    }

    internal class ResilienceHttpClientProvider<TRequest, TResponse> : IResilienceHttpClientProvider<TRequest, TResponse>
    {
        private readonly ISerializerProvider _serializerProvider;
        private readonly IClientHandler<TRequest, TResponse> _clientHandler;
        private readonly IHttpClientProvider<TRequest, TResponse> _httpClientProvider;
        private readonly IHttpMessageBuilder<TRequest, TResponse> _httpMessageBuilder;
        private readonly IMethodResiliencePolicyProvider<TRequest, TResponse> _methodResiliencePolicyProvider;
        private readonly ILogger? _logger;

        public ResilienceHttpClientProvider(
            ISerializerProvider serializerProvider,
            IClientHandler<TRequest, TResponse> clientHandler,
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilder<TRequest, TResponse> httpMessageBuilder,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            ILogger? logger = null)
        {
            _serializerProvider = serializerProvider;
            _clientHandler = clientHandler;
            _httpClientProvider = httpClientProvider;
            _httpMessageBuilder = httpMessageBuilder;
            _methodResiliencePolicyProvider = methodResiliencePolicyProvider;
            _logger = logger;
        }

        public IResilienceHttpClient<TRequest, TResponse> Create(IResiliencePolicyProvider<TRequest, TResponse>? resiliencePolicyProvider)
        {
            return new ResilienceHttpClient<TRequest, TResponse>(
                _serializerProvider,
                _clientHandler,
                _httpClientProvider,
                _httpMessageBuilder,
                resiliencePolicyProvider == null
                    ? _methodResiliencePolicyProvider
                    : new MethodResiliencePolicyProviderAdapter<TRequest, TResponse>(resiliencePolicyProvider),
                _logger);
        }
    }
}
