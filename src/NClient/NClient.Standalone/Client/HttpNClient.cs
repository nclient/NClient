using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Results;
using NClient.Abstractions.Serialization;
using NClient.Standalone.Client.Validation;

namespace NClient.Standalone.Client
{
    internal interface IHttpNClient<TRequest, TResponse>
    {
        Task<TResult> GetResultAsync<TResult>(IHttpRequest httpRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
        Task<TResponse> GetOriginalResponseAsync(IHttpRequest httpRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
        Task<IHttpResponse> GetHttpResponseAsync(IHttpRequest httpRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
        Task<IHttpResponse<TData>> GetHttpResponseAsync<TData>(IHttpRequest httpRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
        Task<IHttpResponseWithError<TError>> GetHttpResponseWithErrorAsync<TError>(IHttpRequest httpRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
        Task<IHttpResponseWithError<TData, TError>> GetHttpResponseWithDataAndErrorAsync<TData, TError>(IHttpRequest httpRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
        Task GetResultAsync(IHttpRequest httpRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
        Task<object?> GetResultAsync(IHttpRequest httpRequest, Type dataType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
        Task<IHttpResponse> GetHttpResponseAsync(IHttpRequest httpRequest, Type dataType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
        Task<IHttpResponse> GetHttpResponseWithErrorAsync(IHttpRequest httpRequest, Type errorType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
        Task<IHttpResponse> GetHttpResponseWithDataAndErrorAsync(IHttpRequest httpRequest, Type dataType, Type errorType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
    }

    internal class HttpNClient<TRequest, TResponse> : IHttpNClient<TRequest, TResponse>
    {
        private readonly ISerializer _serializer;
        private readonly IHttpClient<TRequest, TResponse> _httpClient;
        private readonly IHttpMessageBuilder<TRequest, TResponse> _httpMessageBuilder;
        private readonly IClientHandler<TRequest, TResponse> _clientHandler;
        private readonly IResiliencePolicy<TRequest, TResponse> _resiliencePolicy;
        private readonly IEnumerable<IResultBuilder<TResponse>> _typedResultBuilders;
        private readonly IReadOnlyCollection<IResultBuilder<IHttpResponse>> _resultBuilders;
        private readonly IResponseValidator<TRequest, TResponse> _responseValidator;
        private readonly ILogger? _logger;

        public HttpNClient(
            ISerializer serializer,
            IHttpClient<TRequest, TResponse> httpClient,
            IHttpMessageBuilder<TRequest, TResponse> httpMessageBuilder,
            IClientHandler<TRequest, TResponse> clientHandler,
            IResiliencePolicy<TRequest, TResponse> resiliencePolicy,
            IEnumerable<IResultBuilder<IHttpResponse>> resultBuilders,
            IEnumerable<IResultBuilder<TResponse>> typedResultBuilders,
            IResponseValidator<TRequest, TResponse> responseValidator,
            ILogger? logger)
        {
            _serializer = serializer;
            _httpClient = httpClient;
            _httpMessageBuilder = httpMessageBuilder;
            _clientHandler = clientHandler;
            _resiliencePolicy = resiliencePolicy;
            _typedResultBuilders = typedResultBuilders;
            _resultBuilders = resultBuilders.ToArray();
            _responseValidator = responseValidator;
            _logger = logger;
        }

        public async Task<TResult> GetResultAsync<TResult>(IHttpRequest httpRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            #pragma warning disable 8600, 8603
            return (TResult)await GetResultAsync(httpRequest, typeof(TResult), resiliencePolicy).ConfigureAwait(false);
            #pragma warning restore 8600, 8603
        }

        public async Task<TResponse> GetOriginalResponseAsync(IHttpRequest httpRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            return (await ExecuteAsync(httpRequest, resiliencePolicy).ConfigureAwait(false)).Response;
        }
        
        public async Task<IHttpResponse> GetHttpResponseAsync(IHttpRequest httpRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            var responseContext = await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(() => ExecuteAttemptAsync(httpRequest))
                .ConfigureAwait(false);
            
            return await _httpMessageBuilder
                .BuildResponseAsync(httpRequest, responseContext.Request, responseContext.Response)
                .ConfigureAwait(false);
        }

        public async Task<IHttpResponse<TData>> GetHttpResponseAsync<TData>(IHttpRequest httpRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            return (IHttpResponse<TData>)await GetHttpResponseAsync(httpRequest, dataType: typeof(TData), resiliencePolicy).ConfigureAwait(false);
        }

        public async Task<IHttpResponseWithError<TError>> GetHttpResponseWithErrorAsync<TError>(IHttpRequest httpRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            return (IHttpResponseWithError<TError>)await GetHttpResponseWithErrorAsync(httpRequest, errorType: typeof(TError), resiliencePolicy).ConfigureAwait(false);
        }

        public async Task<IHttpResponseWithError<TData, TError>> GetHttpResponseWithDataAndErrorAsync<TData, TError>(IHttpRequest httpRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            return (IHttpResponseWithError<TData, TError>)await GetHttpResponseWithDataAndErrorAsync(httpRequest, dataType: typeof(TData), errorType: typeof(TData), resiliencePolicy).ConfigureAwait(false);
        }
        
        public async Task GetResultAsync(IHttpRequest httpRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            var responseContext = await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(() => ExecuteAttemptAsync(httpRequest))
                .ConfigureAwait(false);
            
            _responseValidator.Ensure(responseContext);
        }

        public async Task<object?> GetResultAsync(IHttpRequest httpRequest, Type dataType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            var responseContext = await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(() => ExecuteAttemptAsync(httpRequest))
                .ConfigureAwait(false);

            if (_typedResultBuilders.FirstOrDefault(x => x.CanBuild(dataType, responseContext.Response)) is { } typedResultBuilder)
                return await typedResultBuilder
                    .BuildAsync(dataType, responseContext.Response, _serializer)
                    .ConfigureAwait(false);
            
            var httpResponse = await _httpMessageBuilder
                .BuildResponseAsync(httpRequest, responseContext.Request, responseContext.Response)
                .ConfigureAwait(false);

            if (_resultBuilders.FirstOrDefault(x => x.CanBuild(dataType, httpResponse)) is { } resultBuilder)
                return await resultBuilder
                    .BuildAsync(dataType, httpResponse, _serializer)
                    .ConfigureAwait(false);
            
            _responseValidator.Ensure(responseContext);
            
            return _serializer.Deserialize(httpResponse.Content.ToString(), dataType);
        }
        
        public async Task<IHttpResponse> GetHttpResponseAsync(IHttpRequest httpRequest, Type dataType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            var responseContext = await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(() => ExecuteAttemptAsync(httpRequest))
                .ConfigureAwait(false);
            
            var httpResponse = await _httpMessageBuilder
                .BuildResponseAsync(httpRequest, responseContext.Request, responseContext.Response)
                .ConfigureAwait(false);
            
            var dataObject = TryGetDataObject(dataType, httpResponse.Content.ToString(), responseContext);
            return BuildResponseWithData(dataObject, dataType, httpResponse);
        }
        
        public async Task<IHttpResponse> GetHttpResponseWithErrorAsync(IHttpRequest httpRequest, Type errorType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            var responseContext = await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(() => ExecuteAttemptAsync(httpRequest))
                .ConfigureAwait(false);
            
            var httpResponse = await _httpMessageBuilder
                .BuildResponseAsync(httpRequest, responseContext.Request, responseContext.Response)
                .ConfigureAwait(false);
            
            var errorObject = TryGetErrorObject(errorType, httpResponse.Content.ToString(), responseContext);
            return BuildResponseWithError(errorObject, errorType, httpResponse);
        }
        
        public async Task<IHttpResponse> GetHttpResponseWithDataAndErrorAsync(IHttpRequest httpRequest, Type dataType, Type errorType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            var responseContext = await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(() => ExecuteAttemptAsync(httpRequest))
                .ConfigureAwait(false);
            
            var httpResponse = await _httpMessageBuilder
                .BuildResponseAsync(httpRequest, responseContext.Request, responseContext.Response)
                .ConfigureAwait(false);
            
            var dataObject = TryGetDataObject(dataType, httpResponse.Content.ToString(), responseContext);
            var errorObject = TryGetErrorObject(errorType, httpResponse.Content.ToString(), responseContext);
            return BuildResponseWithDataAndError(dataObject, dataType, errorObject, errorType, httpResponse);
        }

        private async Task<IResponseContext<TRequest, TResponse>> ExecuteAsync(IHttpRequest httpRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy)
        {
            return await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(() => ExecuteAttemptAsync(httpRequest))
                .ConfigureAwait(false);
        }

        private async Task<IResponseContext<TRequest, TResponse>> ExecuteAttemptAsync(IHttpRequest httpRequest)
        {
            _logger?.LogDebug("Start sending {requestMethod} request to '{requestUri}'. Request id: '{requestId}'.", httpRequest.Method, httpRequest.Resource, httpRequest.Id);

            TRequest? request;
            TResponse? response;
            try
            {
                _logger?.LogDebug("Start sending request attempt. Request id: '{requestId}'.", httpRequest.Id);
                request = await _httpMessageBuilder
                    .BuildRequestAsync(httpRequest)
                    .ConfigureAwait(false);
                
                await _clientHandler
                    .HandleRequestAsync(request)
                    .ConfigureAwait(false);
                
                response = await _httpClient.ExecuteAsync(request).ConfigureAwait(false);

                response = await _clientHandler
                    .HandleResponseAsync(response)
                    .ConfigureAwait(false);
                
                _logger?.LogDebug("Request attempt finished. Request id: '{requestId}'.", httpRequest.Id);
            }
            catch (Exception e)
            {
                _logger?.LogWarning(e, "Request attempt failed with exception. Request id: '{requestId}'.", httpRequest.Id);
                throw;
            }
            
            _logger?.LogDebug("Response received. Request id: '{requestId}'.", httpRequest.Id);
            return new ResponseContext<TRequest, TResponse>(request, response);
        }
        
        private object? TryGetDataObject(Type dataType, string data, IResponseContext<TRequest, TResponse> responseContext)
        {
            return _responseValidator.IsValid(responseContext)
                ? _serializer.Deserialize(data, dataType)
                : null;
        }

        private object? TryGetErrorObject(Type errorType, string data, IResponseContext<TRequest, TResponse> responseContext)
        {
            return !_responseValidator.IsValid(responseContext)
                ? _serializer.Deserialize(data, errorType)
                : null;
        }
        
        private IHttpResponse BuildResponseWithData(object? data, Type dataType, IHttpResponse httpResponse)
        {
            var genericResponseType = typeof(HttpResponse<>).MakeGenericType(dataType);
            return (IHttpResponse)Activator.CreateInstance(genericResponseType, httpResponse, httpResponse.Request, data);
        }
        
        private IHttpResponse BuildResponseWithError(object? error, Type errorType, IHttpResponse httpResponse)
        {
            var genericResponseType = typeof(HttpResponseWithError<>).MakeGenericType(errorType);
            return (IHttpResponse)Activator.CreateInstance(genericResponseType, httpResponse, httpResponse.Request, error);
        }
        
        private IHttpResponse BuildResponseWithDataAndError(object? data, Type dataType, object? error, Type errorType, IHttpResponse httpResponse)
        {
            var genericResponseType = typeof(HttpResponseWithError<,>).MakeGenericType(dataType, errorType);
            return (IHttpResponse)Activator.CreateInstance(genericResponseType, httpResponse, httpResponse.Request, data, error);
        }
    }
}
