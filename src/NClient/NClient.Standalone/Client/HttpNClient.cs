using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NClient.Providers.Handling;
using NClient.Providers.Resilience;
using NClient.Providers.Results;
using NClient.Providers.Serialization;
using NClient.Providers.Transport;
using NClient.Providers.Validation;

namespace NClient.Standalone.Client
{
    internal interface IHttpNClient<TRequest, TResponse>
    {
        Task<TResult> GetResultAsync<TResult>(IRequest transportRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
        Task<TResponse> GetOriginalResponseAsync(IRequest transportRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
        Task<IResponse> GetHttpResponseAsync(IRequest transportRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
        Task<IResponse<TData>> GetHttpResponseAsync<TData>(IRequest transportRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
        Task<IResponseWithError<TError>> GetHttpResponseWithErrorAsync<TError>(IRequest transportRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
        Task<IResponseWithError<TData, TError>> GetHttpResponseWithDataAndErrorAsync<TData, TError>(IRequest transportRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
        Task GetResultAsync(IRequest transportRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
        Task<object?> GetResultAsync(IRequest transportRequest, Type dataType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
        Task<IResponse> GetHttpResponseAsync(IRequest transportRequest, Type dataType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
        Task<IResponse> GetHttpResponseWithErrorAsync(IRequest transportRequest, Type errorType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
        Task<IResponse> GetHttpResponseWithDataAndErrorAsync(IRequest transportRequest, Type dataType, Type errorType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null);
    }

    internal class HttpNClient<TRequest, TResponse> : IHttpNClient<TRequest, TResponse>
    {
        private readonly ISerializer _serializer;
        private readonly ITransport<TRequest, TResponse> _transport;
        private readonly ITransportMessageBuilder<TRequest, TResponse> _transportMessageBuilder;
        private readonly IClientHandler<TRequest, TResponse> _clientHandler;
        private readonly IResiliencePolicy<TRequest, TResponse> _resiliencePolicy;
        private readonly IEnumerable<IResultBuilder<TResponse>> _typedResultBuilders;
        private readonly IReadOnlyCollection<IResultBuilder<IResponse>> _resultBuilders;
        private readonly IResponseValidator<TRequest, TResponse> _responseValidator;
        private readonly ILogger? _logger;

        public HttpNClient(
            ISerializer serializer,
            ITransport<TRequest, TResponse> transport,
            ITransportMessageBuilder<TRequest, TResponse> transportMessageBuilder,
            IClientHandler<TRequest, TResponse> clientHandler,
            IResiliencePolicy<TRequest, TResponse> resiliencePolicy,
            IEnumerable<IResultBuilder<IResponse>> resultBuilders,
            IEnumerable<IResultBuilder<TResponse>> typedResultBuilders,
            IResponseValidator<TRequest, TResponse> responseValidator,
            ILogger? logger)
        {
            _serializer = serializer;
            _transport = transport;
            _transportMessageBuilder = transportMessageBuilder;
            _clientHandler = clientHandler;
            _resiliencePolicy = resiliencePolicy;
            _typedResultBuilders = typedResultBuilders;
            _resultBuilders = resultBuilders.ToArray();
            _responseValidator = responseValidator;
            _logger = logger;
        }

        public async Task<TResult> GetResultAsync<TResult>(IRequest transportRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            #pragma warning disable 8600, 8603
            return (TResult)await GetResultAsync(transportRequest, typeof(TResult), resiliencePolicy).ConfigureAwait(false);
            #pragma warning restore 8600, 8603
        }

        public async Task<TResponse> GetOriginalResponseAsync(IRequest transportRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            return (await ExecuteAsync(transportRequest, resiliencePolicy).ConfigureAwait(false)).Response;
        }
        
        public async Task<IResponse> GetHttpResponseAsync(IRequest transportRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            var responseContext = await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(() => ExecuteAttemptAsync(transportRequest))
                .ConfigureAwait(false);
            
            return await _transportMessageBuilder
                .BuildResponseAsync(transportRequest, responseContext.Request, responseContext.Response)
                .ConfigureAwait(false);
        }

        public async Task<IResponse<TData>> GetHttpResponseAsync<TData>(IRequest transportRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            return (IResponse<TData>)await GetHttpResponseAsync(transportRequest, dataType: typeof(TData), resiliencePolicy).ConfigureAwait(false);
        }

        public async Task<IResponseWithError<TError>> GetHttpResponseWithErrorAsync<TError>(IRequest transportRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            return (IResponseWithError<TError>)await GetHttpResponseWithErrorAsync(transportRequest, errorType: typeof(TError), resiliencePolicy).ConfigureAwait(false);
        }

        public async Task<IResponseWithError<TData, TError>> GetHttpResponseWithDataAndErrorAsync<TData, TError>(IRequest transportRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            return (IResponseWithError<TData, TError>)await GetHttpResponseWithDataAndErrorAsync(transportRequest, dataType: typeof(TData), errorType: typeof(TData), resiliencePolicy).ConfigureAwait(false);
        }
        
        public async Task GetResultAsync(IRequest transportRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            var responseContext = await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(() => ExecuteAttemptAsync(transportRequest))
                .ConfigureAwait(false);
            
            await _responseValidator.OnFailureAsync(responseContext).ConfigureAwait(false);
        }

        public async Task<object?> GetResultAsync(IRequest transportRequest, Type dataType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            var responseContext = await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(() => ExecuteAttemptAsync(transportRequest))
                .ConfigureAwait(false);

            if (_typedResultBuilders.FirstOrDefault(x => x.CanBuild(dataType, responseContext.Response)) is { } typedResultBuilder)
                return await typedResultBuilder
                    .BuildAsync(dataType, responseContext.Response, _serializer)
                    .ConfigureAwait(false);
            
            var httpResponse = await _transportMessageBuilder
                .BuildResponseAsync(transportRequest, responseContext.Request, responseContext.Response)
                .ConfigureAwait(false);

            if (_resultBuilders.FirstOrDefault(x => x.CanBuild(dataType, httpResponse)) is { } resultBuilder)
                return await resultBuilder
                    .BuildAsync(dataType, httpResponse, _serializer)
                    .ConfigureAwait(false);
            
            await _responseValidator.OnFailureAsync(responseContext).ConfigureAwait(false);
            
            return _serializer.Deserialize(httpResponse.Content.ToString(), dataType);
        }
        
        public async Task<IResponse> GetHttpResponseAsync(IRequest transportRequest, Type dataType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            var responseContext = await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(() => ExecuteAttemptAsync(transportRequest))
                .ConfigureAwait(false);
            
            var httpResponse = await _transportMessageBuilder
                .BuildResponseAsync(transportRequest, responseContext.Request, responseContext.Response)
                .ConfigureAwait(false);
            
            var dataObject = TryGetDataObject(dataType, httpResponse.Content.ToString(), responseContext);
            return BuildResponseWithData(dataObject, dataType, httpResponse);
        }
        
        public async Task<IResponse> GetHttpResponseWithErrorAsync(IRequest transportRequest, Type errorType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            var responseContext = await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(() => ExecuteAttemptAsync(transportRequest))
                .ConfigureAwait(false);
            
            var httpResponse = await _transportMessageBuilder
                .BuildResponseAsync(transportRequest, responseContext.Request, responseContext.Response)
                .ConfigureAwait(false);
            
            var errorObject = TryGetErrorObject(errorType, httpResponse.Content.ToString(), responseContext);
            return BuildResponseWithError(errorObject, errorType, httpResponse);
        }
        
        public async Task<IResponse> GetHttpResponseWithDataAndErrorAsync(IRequest transportRequest, Type dataType, Type errorType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null)
        {
            var responseContext = await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(() => ExecuteAttemptAsync(transportRequest))
                .ConfigureAwait(false);
            
            var httpResponse = await _transportMessageBuilder
                .BuildResponseAsync(transportRequest, responseContext.Request, responseContext.Response)
                .ConfigureAwait(false);
            
            var dataObject = TryGetDataObject(dataType, httpResponse.Content.ToString(), responseContext);
            var errorObject = TryGetErrorObject(errorType, httpResponse.Content.ToString(), responseContext);
            return BuildResponseWithDataAndError(dataObject, dataType, errorObject, errorType, httpResponse);
        }

        private async Task<IResponseContext<TRequest, TResponse>> ExecuteAsync(IRequest transportRequest, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy)
        {
            return await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(() => ExecuteAttemptAsync(transportRequest))
                .ConfigureAwait(false);
        }

        private async Task<IResponseContext<TRequest, TResponse>> ExecuteAttemptAsync(IRequest request)
        {
            _logger?.LogDebug("Start sending {requestMethod} request to '{requestUri}'. Request id: '{requestId}'.", request.Method, request.Resource, request.Id);

            TRequest? transportRequest;
            TResponse? transportResponse;
            try
            {
                _logger?.LogDebug("Start sending request attempt. Request id: '{requestId}'.", request.Id);
                transportRequest = await _transportMessageBuilder
                    .BuildTransportRequestAsync(request)
                    .ConfigureAwait(false);
                
                await _clientHandler
                    .HandleRequestAsync(transportRequest)
                    .ConfigureAwait(false);
                
                transportResponse = await _transport.ExecuteAsync(transportRequest).ConfigureAwait(false);

                transportResponse = await _clientHandler
                    .HandleResponseAsync(transportResponse)
                    .ConfigureAwait(false);
                
                _logger?.LogDebug("Request attempt finished. Request id: '{requestId}'.", request.Id);
            }
            catch (Exception e)
            {
                _logger?.LogWarning(e, "Request attempt failed with exception. Request id: '{requestId}'.", request.Id);
                throw;
            }
            
            _logger?.LogDebug("Response received. Request id: '{requestId}'.", request.Id);
            return new ResponseContext<TRequest, TResponse>(transportRequest, transportResponse);
        }
        
        private object? TryGetDataObject(Type dataType, string data, IResponseContext<TRequest, TResponse> responseContext)
        {
            return _responseValidator.IsSuccess(responseContext)
                ? _serializer.Deserialize(data, dataType)
                : null;
        }

        private object? TryGetErrorObject(Type errorType, string data, IResponseContext<TRequest, TResponse> responseContext)
        {
            return !_responseValidator.IsSuccess(responseContext)
                ? _serializer.Deserialize(data, errorType)
                : null;
        }
        
        private static IResponse BuildResponseWithData(object? data, Type dataType, IResponse transportResponse)
        {
            var genericResponseType = typeof(Response<>).MakeGenericType(dataType);
            return (IResponse)Activator.CreateInstance(genericResponseType, transportResponse, transportResponse.Request, data);
        }
        
        private static IResponse BuildResponseWithError(object? error, Type errorType, IResponse transportResponse)
        {
            var genericResponseType = typeof(ResponseWithError<>).MakeGenericType(errorType);
            return (IResponse)Activator.CreateInstance(genericResponseType, transportResponse, transportResponse.Request, error);
        }
        
        private static IResponse BuildResponseWithDataAndError(object? data, Type dataType, object? error, Type errorType, IResponse transportResponse)
        {
            var genericResponseType = typeof(ResponseWithError<,>).MakeGenericType(dataType, errorType);
            return (IResponse)Activator.CreateInstance(genericResponseType, transportResponse, transportResponse.Request, data, error);
        }
    }
}
