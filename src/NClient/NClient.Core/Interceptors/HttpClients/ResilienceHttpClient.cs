using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Invocation;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Core.Interceptors.HttpResponsePopulation;

namespace NClient.Core.Interceptors.HttpClients
{
    internal interface IResilienceHttpClient
    {
        Task<HttpResponse> ExecuteAsync(HttpRequest request, MethodInvocation methodInvocation);
    }

    internal class ResilienceHttpClient : IResilienceHttpClient
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly ISerializerProvider _serializerProvider;
        private readonly IHttpResponsePopulater _httpResponsePopulater;
        private readonly IResiliencePolicyProvider _resiliencePolicyProvider;
        private readonly ILogger? _logger;

        public ResilienceHttpClient(
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider,
            IHttpResponsePopulater httpResponsePopulater,
            IResiliencePolicyProvider resiliencePolicyProvider,
            ILogger? logger)
        {
            _httpClientProvider = httpClientProvider;
            _serializerProvider = serializerProvider;
            _httpResponsePopulater = httpResponsePopulater;
            _resiliencePolicyProvider = resiliencePolicyProvider;
            _logger = logger;
        }

        public async Task<HttpResponse> ExecuteAsync(HttpRequest request, MethodInvocation methodInvocation)
        {
            _logger?.LogDebug("Start sending {requestMethod} request to '{requestUri}'. Request id: '{requestId}'.", request.Method, request.Uri, request.Id);

            return await _resiliencePolicyProvider
                .Create()
                .ExecuteAsync(() => ExecuteAttemptAsync(request, methodInvocation))
                .ConfigureAwait(false);
        }

        private async Task<(HttpResponse, MethodInvocation)> ExecuteAttemptAsync(HttpRequest request, MethodInvocation methodInvocation)
        {
            var serializer = _serializerProvider.Create();
            var client = _httpClientProvider.Create(serializer);
            HttpResponse response;
            try
            {
                _logger?.LogDebug("Start sending request attempt. Request id: '{requestId}'.", request.Id);
                response = await client.ExecuteAsync(request).ConfigureAwait(false);
                _logger?.LogDebug("Request attempt finished with code {responseStatusCode} received. Request id: '{requestId}'.", response.StatusCode, request.Id);
            }
            catch (Exception e)
            {
                _logger?.LogWarning(e, "Request attempt failed with exception. Request id: '{requestId}'.", request.Id);
                throw;
            }
            
            var populatedResponse = _httpResponsePopulater.Populate(response, methodInvocation.ResultType);
            _logger?.LogDebug("Response with code {responseStatusCode} received. Request id: '{requestId}'.", response.StatusCode, response.Request.Id);
            return (populatedResponse, methodInvocation);
        }
    }
}