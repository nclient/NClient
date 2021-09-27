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
    internal interface IResilienceHttpClient<TRequest, TResponse>
    {
        Task<TResponse> ExecuteAsync(Guid requestId, TRequest request, MethodInvocation methodInvocation);
    }

    internal class ResilienceHttpClient<TRequest, TResponse> : IResilienceHttpClient<TRequest, TResponse>
    {
        private readonly ISerializerProvider _serializerProvider;
        private readonly IClientHandler<TRequest, TResponse> _clientHandler;
        private readonly IHttpClientProvider<TRequest, TResponse> _httpClientProvider;
        private readonly IMethodResiliencePolicyProvider<TResponse> _methodResiliencePolicyProvider;
        private readonly ILogger? _logger;

        public ResilienceHttpClient(
            ISerializerProvider serializerProvider,
            IClientHandler<TRequest, TResponse> clientHandler,
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IMethodResiliencePolicyProvider<TResponse> methodResiliencePolicyProvider,
            ILogger? logger)
        {
            _serializerProvider = serializerProvider;
            _clientHandler = clientHandler;
            _httpClientProvider = httpClientProvider;
            _methodResiliencePolicyProvider = methodResiliencePolicyProvider;
            _logger = logger;
        }

        public async Task<TResponse> ExecuteAsync(Guid requestId, TRequest request, MethodInvocation methodInvocation)
        {
            await _clientHandler
                .HandleRequestAsync(request, methodInvocation)
                .ConfigureAwait(false);
            
            var response = await _methodResiliencePolicyProvider
                .Create(methodInvocation.MethodInfo)
                .ExecuteAsync(() => ExecuteAttemptAsync(requestId, request, methodInvocation))
                .ConfigureAwait(false);
            
            return await _clientHandler
                .HandleResponseAsync(response, methodInvocation)
                .ConfigureAwait(false);
        }

        private async Task<ResponseContext<TResponse>> ExecuteAttemptAsync(Guid requestId, TRequest request, MethodInvocation methodInvocation)
        {
            var serializer = _serializerProvider.Create();
            var client = _httpClientProvider.Create(serializer);
            TResponse response;
            try
            {
                _logger?.LogDebug("Start sending request attempt. Request id: '{requestId}'.", requestId);
                response = await client.ExecuteAsync(request).ConfigureAwait(false);
                _logger?.LogDebug("Request attempt finished. Request id: '{requestId}'.", requestId);
            }
            catch (Exception e)
            {
                _logger?.LogWarning(e, "Request attempt failed with exception. Request id: '{requestId}'.", requestId);
                throw;
            }
            
            _logger?.LogDebug("Response received. Request id: '{requestId}'.", requestId);
            return new ResponseContext<TResponse>(response, methodInvocation);
        }
    }
}
