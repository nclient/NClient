using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;

namespace NClient.Core.HttpClients
{
    public class ResilienceHttpClient : IHttpClient
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly IResiliencePolicyProvider _resiliencePolicyProvider;
        private readonly ILogger? _logger;

        public ResilienceHttpClient(
            IHttpClientProvider httpClientProvider,
            IResiliencePolicyProvider resiliencePolicyProvider,
            ILogger? logger)
        {
            _httpClientProvider = httpClientProvider;
            _resiliencePolicyProvider = resiliencePolicyProvider;
            _logger = logger;
        }

        public async Task<HttpResponse> ExecuteAsync(HttpRequest request, Type? bodyType = null)
        {
            _logger?.LogDebug("Start sending {requestMethod} request to '{requestUri}'. Request id: '{requestId}'.", request.Method, request.Uri, request.Id);

            var response = await _resiliencePolicyProvider
                .Create()
                .ExecuteAsync(() => ExecuteAttemptAsync(request, bodyType))
                .ConfigureAwait(false);

            _logger?.LogDebug("Response with code {responseStatusCode} received. Request id: '{requestId}'.", response.StatusCode, response.Request.Id);

            return response;
        }

        private async Task<HttpResponse> ExecuteAttemptAsync(HttpRequest request, Type? bodyType = null)
        {
            var client = _httpClientProvider.Create();
            try
            {
                _logger?.LogDebug("Start sending request attempt. Request id: '{requestId}'.", request.Id);
                var response = await client.ExecuteAsync(request, bodyType).ConfigureAwait(false);
                _logger?.LogDebug("Request attempt finished with code {responseStatusCode} received. Request id: '{requestId}'.", response.StatusCode, request.Id);
                return response;
            }
            catch (Exception e)
            {
                _logger?.LogWarning(e, "Request attempt failed with exception. Request id: '{requestId}'.", request.Id);
                throw;
            }
        }
    }
}