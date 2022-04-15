using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NClient.Common.Helpers;
using NClient.Providers.Handling;
using NClient.Providers.Mapping;
using NClient.Providers.Resilience;
using NClient.Providers.Serialization;
using NClient.Providers.Transport;
using NClient.Providers.Validation;

namespace NClient.Standalone.Client
{
    internal interface ITransportNClient<TRequest, TResponse>
    {
        TimeSpan Timeout { get; }
        Task<TResponse> GetOriginalResponseAsync(IRequest request, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default);
        Task GetResultAsync(IRequest request, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default);
        Task<object?> GetResultAsync(IRequest request, Type dataType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default);
        Task<IResponse> GetResponseAsync(IRequest request, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default); 
        Task<IResponse> GetResponseWithDataAsync(IRequest request, Type dataType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default);
        Task<IResponse> GetResponseWithErrorAsync(IRequest request, Type errorType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default);
        Task<IResponse> GetResponseWithDataOrErrorAsync(IRequest request, Type dataType, Type errorType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default);
    }

    internal class TransportNClient<TRequest, TResponse> : ITransportNClient<TRequest, TResponse>
    {
        private readonly ISerializer _serializer;
        private readonly ITransport<TRequest, TResponse> _transport;
        private readonly ITransportRequestBuilder<TRequest, TResponse> _transportRequestBuilder;
        private readonly IResponseBuilder<TRequest, TResponse> _responseBuilder;
        private readonly IClientHandler<TRequest, TResponse> _clientHandler;
        private readonly IResiliencePolicy<TRequest, TResponse> _resiliencePolicy;
        private readonly IReadOnlyCollection<IResponseMapper<TRequest, TResponse>> _transportResponseMappers;
        private readonly IReadOnlyCollection<IResponseMapper<IRequest, IResponse>> _responseMappers;
        private readonly IResponseValidator<TRequest, TResponse> _responseValidator;
        private readonly ILogger? _logger;

        public TimeSpan Timeout => _transport.Timeout;

        public TransportNClient(
            ISerializer serializer,
            ITransport<TRequest, TResponse> transport,
            ITransportRequestBuilder<TRequest, TResponse> transportRequestBuilder,
            IResponseBuilder<TRequest, TResponse> responseBuilder,
            IClientHandler<TRequest, TResponse> clientHandler,
            IResiliencePolicy<TRequest, TResponse> resiliencePolicy,
            IEnumerable<IResponseMapper<IRequest, IResponse>> responseMappers,
            IEnumerable<IResponseMapper<TRequest, TResponse>> transportResponseMappers,
            IResponseValidator<TRequest, TResponse> responseValidator,
            ILogger? logger)
        {
            _serializer = serializer;
            _transport = transport;
            _transportRequestBuilder = transportRequestBuilder;
            _responseBuilder = responseBuilder;
            _clientHandler = clientHandler;
            _resiliencePolicy = resiliencePolicy;
            _transportResponseMappers = transportResponseMappers.ToArray();
            _responseMappers = responseMappers.ToArray();
            _responseValidator = responseValidator;
            _logger = logger;
        }

        public async Task<TResponse> GetOriginalResponseAsync(IRequest request, 
            IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default)
        {
            return (await ExecuteAsync(request, resiliencePolicy, cancellationToken).ConfigureAwait(false)).Response;
        }
        
        public async Task GetResultAsync(IRequest request, 
            IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default)
        {
            var transportResponseContext = await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(token => ExecuteAttemptAsync(request, token), cancellationToken)
                .ConfigureAwait(false);
            
            if (!_responseValidator.IsSuccess(transportResponseContext))
                await _responseValidator.OnFailureAsync(transportResponseContext).ConfigureAwait(false);
        }

        public async Task<object?> GetResultAsync(IRequest request, Type dataType, 
            IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default)
        {
            var transportResponseContext = await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(token => ExecuteAttemptAsync(request, token), cancellationToken)
                .ConfigureAwait(false);
            
            if (_transportResponseMappers.FirstOrDefault(x => x.CanMap(dataType, transportResponseContext)) is { } transportResponseMapper)
                return await transportResponseMapper
                    .MapAsync(dataType, transportResponseContext, cancellationToken)
                    .ConfigureAwait(false);
            
            var response = await _responseBuilder
                .BuildAsync(request, transportResponseContext, allocateMemoryForContent: true, cancellationToken)
                .ConfigureAwait(false);
            var responseContext = new ResponseContext<IRequest, IResponse>(request, response);
            
            if (_responseMappers.FirstOrDefault(x => x.CanMap(dataType, responseContext)) is { } responseMapper)
                return await responseMapper
                    .MapAsync(dataType, responseContext, cancellationToken)
                    .ConfigureAwait(false);
            
            if (!_responseValidator.IsSuccess(transportResponseContext))
                await _responseValidator.OnFailureAsync(transportResponseContext).ConfigureAwait(false);
            
            var stringContent = await responseContext.Response.Content.Stream
                .ReadToEndAsync(responseContext.Response.Content.Encoding, cancellationToken)
                .ConfigureAwait(false);
            
            return _serializer.Deserialize(stringContent, dataType);
        }
        
        public async Task<IResponse> GetResponseAsync(IRequest request, 
            IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default)
        {
            var transportResponseContext = await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(token => ExecuteAttemptAsync(request, token), cancellationToken)
                .ConfigureAwait(false);
            
            return await _responseBuilder
                .BuildAsync(request, transportResponseContext, allocateMemoryForContent: false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IResponse> GetResponseWithDataAsync(IRequest request, Type dataType, 
            IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default)
        {
            var transportResponseContext = await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(token => ExecuteAttemptAsync(request, token), cancellationToken)
                .ConfigureAwait(false);
            
            var response = await _responseBuilder
                .BuildAsync(request, transportResponseContext, allocateMemoryForContent: true, cancellationToken)
                .ConfigureAwait(false);

            var stringContent = await response.Content.Stream
                .ReadToEndAsync(response.Content.Encoding, cancellationToken)
                .ConfigureAwait(false);

            var dataObject = TryGetDataObject(dataType, stringContent, transportResponseContext);
            return BuildResponseWithData(dataObject, dataType, response);
        }
        
        public async Task<IResponse> GetResponseWithErrorAsync(IRequest request, Type errorType, 
            IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default)
        {
            var transportResponseContext = await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(token => ExecuteAttemptAsync(request, token), cancellationToken)
                .ConfigureAwait(false);
            
            var response = await _responseBuilder
                .BuildAsync(request, transportResponseContext, allocateMemoryForContent: true, cancellationToken)
                .ConfigureAwait(false);

            var stringContent = await response.Content.Stream
                .ReadToEndAsync(response.Content.Encoding, cancellationToken)
                .ConfigureAwait(false);
            
            var errorObject = TryGetErrorObject(errorType, stringContent, transportResponseContext);
            return BuildResponseWithError(errorObject, errorType, response);
        }
        
        public async Task<IResponse> GetResponseWithDataOrErrorAsync(IRequest request, Type dataType, Type errorType, 
            IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default)
        {
            var transportResponseContext = await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(token => ExecuteAttemptAsync(request, token), cancellationToken)
                .ConfigureAwait(false);
            
            var response = await _responseBuilder
                .BuildAsync(request, transportResponseContext, allocateMemoryForContent: true, cancellationToken)
                .ConfigureAwait(false);

            var stringContent = await response.Content.Stream
                .ReadToEndAsync(response.Content.Encoding, cancellationToken)
                .ConfigureAwait(false);
            
            var dataObject = TryGetDataObject(dataType, stringContent, transportResponseContext);
            var errorObject = TryGetErrorObject(errorType, stringContent, transportResponseContext);
            return BuildResponseWithDataOrError(dataObject, dataType, errorObject, errorType, response);
        }

        private async Task<IResponseContext<TRequest, TResponse>> ExecuteAsync(IRequest transportRequest, 
            IResiliencePolicy<TRequest, TResponse>? resiliencePolicy, CancellationToken cancellationToken = default)
        {
            return await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(token => ExecuteAttemptAsync(transportRequest, token), cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task<IResponseContext<TRequest, TResponse>> ExecuteAttemptAsync(IRequest request, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Start sending '{RequestMethod}' request to '{RequestUri}'. Request id: '{RequestId}'", request.Type, request.Resource, request.Id);

            TRequest? transportRequest;
            TResponse? transportResponse;
            try
            {
                _logger?.LogDebug("Start sending request attempt (request id: '{RequestId}')", request.Id);
                transportRequest = await _transportRequestBuilder
                    .BuildAsync(request, cancellationToken)
                    .ConfigureAwait(false);
                
                await _clientHandler
                    .HandleRequestAsync(transportRequest, cancellationToken)
                    .ConfigureAwait(false);
                
                transportResponse = await _transport.ExecuteAsync(transportRequest, cancellationToken).ConfigureAwait(false);

                transportResponse = await _clientHandler
                    .HandleResponseAsync(transportResponse, cancellationToken)
                    .ConfigureAwait(false);
                
                _logger?.LogDebug("Request attempt finished (request id: '{RequestId}')", request.Id);
            }
            catch (Exception e)
            {
                _logger?.LogWarning(e, "Request attempt failed with exception (request id: '{RequestId}')", request.Id);
                throw;
            }
            
            _logger?.LogDebug("Response received (request id: '{RequestId}')    ", request.Id);
            return new ResponseContext<TRequest, TResponse>(transportRequest, transportResponse);
        }

        private object? TryGetDataObject(Type dataType, string data, IResponseContext<TRequest, TResponse> transportResponseContext)
        {
            return _responseValidator.IsSuccess(transportResponseContext)
                ? _serializer.Deserialize(data, dataType)
                : null;
        }

        private object? TryGetErrorObject(Type errorType, string data, IResponseContext<TRequest, TResponse> transportResponseContext)
        {
            return !_responseValidator.IsSuccess(transportResponseContext)
                ? _serializer.Deserialize(data, errorType)
                : null;
        }
        
        private static IResponse BuildResponseWithData(object? data, Type dataType, IResponse response)
        {
            var genericResponseType = typeof(Response<>).MakeGenericType(dataType);
            return (IResponse) Activator.CreateInstance(genericResponseType, response, response.Request, data);
        }
        
        private static IResponse BuildResponseWithError(object? error, Type errorType, IResponse response)
        {
            var genericResponseType = typeof(ResponseWithError<>).MakeGenericType(errorType);
            return (IResponse) Activator.CreateInstance(genericResponseType, response, response.Request, error);
        }
        
        private static IResponse BuildResponseWithDataOrError(object? data, Type dataType, object? error, Type errorType, IResponse response)
        {
            var genericResponseType = typeof(ResponseWithError<,>).MakeGenericType(dataType, errorType);
            return (IResponse) Activator.CreateInstance(genericResponseType, response, response.Request, data, error);
        }
    }
}
