using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Invocation;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;

namespace NClient.Core.Interceptors.HttpClients
{
    internal interface IResilienceHttpClient<TResponse>
    {
        Task<TResponse> ExecuteAsync(HttpRequest request, MethodInvocation methodInvocation);
    }

    internal class ResilienceHttpClient<TRequest, TResponse> : IResilienceHttpClient<TResponse>
    {
        private readonly ISerializerProvider _serializerProvider;
        private readonly IClientHandler<TRequest, TResponse> _clientHandler;
        private readonly IHttpClientProvider<TRequest, TResponse> _httpClientProvider;
        private readonly IHttpMessageBuilder<TRequest, TResponse> _httpMessageBuilder;
        private readonly IHttpClientExceptionFactory<TRequest, TResponse> _httpClientExceptionFactory;
        private readonly IMethodResiliencePolicyProvider<TResponse> _methodResiliencePolicyProvider;
        private readonly ILogger? _logger;

        public ResilienceHttpClient(
            ISerializerProvider serializerProvider,
            IClientHandler<TRequest, TResponse> clientHandler,
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilder<TRequest, TResponse> httpMessageBuilder,
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory,
            IMethodResiliencePolicyProvider<TResponse> methodResiliencePolicyProvider,
            ILogger? logger)
        {
            _serializerProvider = serializerProvider;
            _clientHandler = clientHandler;
            _httpClientProvider = httpClientProvider;
            _httpMessageBuilder = httpMessageBuilder;
            _httpClientExceptionFactory = httpClientExceptionFactory;
            _methodResiliencePolicyProvider = methodResiliencePolicyProvider;
            _logger = logger;
        }

        public async Task<TResponse> ExecuteAsync(HttpRequest httpRequest, MethodInvocation methodInvocation)
        {
            return await _methodResiliencePolicyProvider
                .Create(methodInvocation.MethodInfo)
                .ExecuteAsync(() => ExecuteAttemptAsync(httpRequest, methodInvocation))
                .ConfigureAwait(false);
        }

        private async Task<ResponseContext<TResponse>> ExecuteAttemptAsync(HttpRequest httpRequest, MethodInvocation methodInvocation)
        {
            _logger?.LogDebug("Start sending {requestMethod} request to '{requestUri}'. Request id: '{requestId}'.", httpRequest.Method, httpRequest.Resource, httpRequest.Id);

            var serializer = _serializerProvider.Create();
            var client = _httpClientProvider.Create(serializer);
            
            TRequest? request = default;
            TResponse? response = default;
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
                if (_httpClientExceptionFactory.TryCreate(request, response, e) is Exception httpClientException)
                    throw httpClientException;
                throw;
            }
            
            _logger?.LogDebug("Response received. Request id: '{requestId}'.", httpRequest.Id);
            return new ResponseContext<TResponse>(response, methodInvocation);
        }
    }
}
