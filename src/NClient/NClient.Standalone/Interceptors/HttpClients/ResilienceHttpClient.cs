﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Invocation;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;

namespace NClient.Standalone.Interceptors.HttpClients
{
    internal interface IResilienceHttpClient<TRequest, TResponse>
    {
        Task<IResponseContext<TRequest, TResponse>> ExecuteAsync(IHttpRequest request, IMethodInvocation methodInvocation);
    }

    internal class ResilienceHttpClient<TRequest, TResponse> : IResilienceHttpClient<TRequest, TResponse>
    {
        private readonly ISerializerProvider _serializerProvider;
        private readonly IClientHandler<TRequest, TResponse> _clientHandler;
        private readonly IHttpClientProvider<TRequest, TResponse> _httpClientProvider;
        private readonly IHttpMessageBuilder<TRequest, TResponse> _httpMessageBuilder;
        private readonly IMethodResiliencePolicyProvider<TRequest, TResponse> _methodResiliencePolicyProvider;
        private readonly ILogger? _logger;

        public ResilienceHttpClient(
            ISerializerProvider serializerProvider,
            IClientHandler<TRequest, TResponse> clientHandler,
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilder<TRequest, TResponse> httpMessageBuilder,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            ILogger? logger)
        {
            _serializerProvider = serializerProvider;
            _clientHandler = clientHandler;
            _httpClientProvider = httpClientProvider;
            _httpMessageBuilder = httpMessageBuilder;
            _methodResiliencePolicyProvider = methodResiliencePolicyProvider;
            _logger = logger;
        }

        public async Task<IResponseContext<TRequest, TResponse>> ExecuteAsync(IHttpRequest httpRequest, IMethodInvocation methodInvocation)
        {
            return await _methodResiliencePolicyProvider
                .Create(methodInvocation.MethodInfo, httpRequest)
                .ExecuteAsync(() => ExecuteAttemptAsync(httpRequest, methodInvocation))
                .ConfigureAwait(false);
        }

        private async Task<IResponseContext<TRequest, TResponse>> ExecuteAttemptAsync(IHttpRequest httpRequest, IMethodInvocation methodInvocation)
        {
            _logger?.LogDebug("Start sending {requestMethod} request to '{requestUri}'. Request id: '{requestId}'.", httpRequest.Method, httpRequest.Resource, httpRequest.Id);

            var serializer = _serializerProvider.Create();
            var client = _httpClientProvider.Create(serializer);
            
            TRequest? request;
            TResponse? response;
            try
            {
                _logger?.LogDebug("Start sending request attempt. Request id: '{requestId}'.", httpRequest.Id);
                request = await _httpMessageBuilder
                    .BuildRequestAsync(httpRequest)
                    .ConfigureAwait(false);
                
                await _clientHandler
                    .HandleRequestAsync(request, methodInvocation)
                    .ConfigureAwait(false);
                
                response = await client.ExecuteAsync(request).ConfigureAwait(false);

                response = await _clientHandler
                    .HandleResponseAsync(response, methodInvocation)
                    .ConfigureAwait(false);
                
                _logger?.LogDebug("Request attempt finished. Request id: '{requestId}'.", httpRequest.Id);
            }
            catch (Exception e)
            {
                _logger?.LogWarning(e, "Request attempt failed with exception. Request id: '{requestId}'.", httpRequest.Id);
                throw;
            }
            
            _logger?.LogDebug("Response received. Request id: '{requestId}'.", httpRequest.Id);
            return new ResponseContext<TRequest, TResponse>(request, response, methodInvocation);
        }
    }
}
