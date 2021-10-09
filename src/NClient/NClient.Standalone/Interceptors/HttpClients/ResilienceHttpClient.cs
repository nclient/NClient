using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Invocation;
using NClient.Abstractions.Mapping;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Providers.Results.HttpMessages;
using NClient.Standalone.Interceptors.Validation;

namespace NClient.Standalone.Interceptors.HttpClients
{
    internal interface IResilienceHttpClient<TResponse>
    {
        Task<TResponse> ExecuteAsync(IHttpRequest request, IMethodInvocation methodInvocation);
    }

    internal class ResilienceHttpClient<TRequest, TResponse> : IResilienceHttpClient<TResponse>
    {
        private readonly ISerializerProvider _serializerProvider;
        private readonly IClientHandler<TRequest, TResponse> _clientHandler;
        private readonly IResponseValidator<TRequest, TResponse> _responseValidator;
        private readonly IHttpClientProvider<TRequest, TResponse> _httpClientProvider;
        private readonly IHttpMessageBuilder<TRequest, TResponse> _httpMessageBuilder;
        private readonly IReadOnlyCollection<IResponseMapper> _responseMappers;
        private readonly IMethodResiliencePolicyProvider<TRequest, TResponse> _methodResiliencePolicyProvider;
        private readonly ILogger? _logger;

        public ResilienceHttpClient(
            ISerializerProvider serializerProvider,
            IClientHandler<TRequest, TResponse> clientHandler,
            IResponseValidator<TRequest, TResponse> responseValidator,
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilder<TRequest, TResponse> httpMessageBuilder,
            IEnumerable<IResponseMapper> responseMappers,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            ILogger? logger)
        {
            _serializerProvider = serializerProvider;
            _clientHandler = clientHandler;
            _responseValidator = responseValidator;
            _httpClientProvider = httpClientProvider;
            _httpMessageBuilder = httpMessageBuilder;
            _responseMappers = responseMappers.ToArray();
            _methodResiliencePolicyProvider = methodResiliencePolicyProvider;
            _logger = logger;
        }

        public async Task<TResponse> ExecuteAsync(IHttpRequest httpRequest, IMethodInvocation methodInvocation)
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
            var responseContext = new ResponseContext<TRequest, TResponse>(request, response, methodInvocation);
            if (responseContext.MethodInvocation.ResultType == typeof(TResponse))
                return responseContext;
            if (responseContext.MethodInvocation.ResultType == typeof(HttpResponse) || responseContext.MethodInvocation.ResultType == typeof(IHttpResponse))
                return responseContext;
            if (_responseMappers.Any(x => x.CanMapTo(responseContext.MethodInvocation.ResultType)))
                return responseContext;
            return _responseValidator.Ensure(responseContext);
        }
    }
}
