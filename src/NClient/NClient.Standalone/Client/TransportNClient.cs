using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NClient.Providers.Caching;
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
        Task<TResponse> GetTransportResponseAsync(IRequest request, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default);
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
        private readonly IResponseMapper<TRequest, TResponse> _transportResponseMapper;
        private readonly IResponseMapper<IRequest, IResponse> _responseMapper;
        private readonly IResponseValidator<TRequest, TResponse> _responseValidator;
        private readonly IResponseCacheWorker? _transportResponseCacheWorker;        
        private readonly ILogger _logger;

        public TimeSpan Timeout => _transport.Timeout;

        public TransportNClient(
            ISerializer serializer,
            ITransport<TRequest, TResponse> transport,
            ITransportRequestBuilder<TRequest, TResponse> transportRequestBuilder,
            IResponseBuilder<TRequest, TResponse> responseBuilder,
            IClientHandler<TRequest, TResponse> clientHandler,
            IResiliencePolicy<TRequest, TResponse> resiliencePolicy,
            IResponseMapper<IRequest, IResponse> responseMapper,
            IResponseMapper<TRequest, TResponse> transportResponseMapper,
            IResponseValidator<TRequest, TResponse> responseValidator,            
            IResponseCacheWorker? transportResponseCacheWorker,
            ILogger logger)
        {
            _serializer = serializer;
            _transport = transport;
            _transportRequestBuilder = transportRequestBuilder;
            _responseBuilder = responseBuilder;
            _clientHandler = clientHandler;
            _resiliencePolicy = resiliencePolicy;
            _responseMapper = responseMapper;
            _transportResponseMapper = transportResponseMapper;
            _responseValidator = responseValidator;
            _transportResponseCacheWorker = transportResponseCacheWorker;
            _logger = logger;
        }

        public async Task<TResponse> GetTransportResponseAsync(IRequest request, 
            IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default)
        {
            using var requestLogScope = BeginLogScope(request);
            LogRequest(request);
            
            var transportResponseContext = await ExecuteAsync(request, resiliencePolicy, cancellationToken)
                .ConfigureAwait(false);
            LogTransportResponse(request);
            
            return transportResponseContext.Response;
        }
        
        public async Task GetResultAsync(IRequest request, 
            IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default)
        {
            using var requestLogScope = BeginLogScope(request);
            LogRequest(request);
            
            var transportResponseContext = await ExecuteAsync(request, resiliencePolicy, cancellationToken)
                .ConfigureAwait(false);
            LogTransportResponse(request);

            if (!_responseValidator.IsSuccess(transportResponseContext))
                await _responseValidator.OnFailureAsync(transportResponseContext).ConfigureAwait(false);
        }

        public async Task<object?> GetResultAsync(IRequest request, Type dataType, 
            IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default)
        {
            using var requestLogScope = BeginLogScope(request);
            LogRequest(request);
            
            var transportResponseContext = await ExecuteAsync(request, resiliencePolicy, cancellationToken)
                .ConfigureAwait(false);
            
            if (_transportResponseMapper.CanMap(dataType, transportResponseContext))
                return await _transportResponseMapper
                    .MapAsync(dataType, transportResponseContext, cancellationToken)
                    .ConfigureAwait(false);
            
            var response = await _responseBuilder
                .BuildAsync(request, transportResponseContext, allocateMemoryForContent: true, cancellationToken)
                .ConfigureAwait(false);
            LogResponse(response);
            
            var responseContext = new ResponseContext<IRequest, IResponse>(request, response);
            
            if (_responseMapper.CanMap(dataType, responseContext))
                return await _responseMapper
                    .MapAsync(dataType, responseContext, cancellationToken)
                    .ConfigureAwait(false);
            
            if (!_responseValidator.IsSuccess(transportResponseContext))
                await _responseValidator.OnFailureAsync(transportResponseContext).ConfigureAwait(false);
            
            return await TryGetDataObject(dataType, response.Content, cancellationToken).ConfigureAwait(false);
        }
        
        public async Task<IResponse> GetResponseAsync(IRequest request, 
            IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default)
        {
            using var requestLogScope = BeginLogScope(request);
            LogRequest(request);
            
            var transportResponseContext = await ExecuteAsync(request, resiliencePolicy, cancellationToken)
                .ConfigureAwait(false);
            
            var response = await _responseBuilder
                .BuildAsync(request, transportResponseContext, allocateMemoryForContent: false, cancellationToken)
                .ConfigureAwait(false);
            LogResponse(response);

            return response;
        }

        public async Task<IResponse> GetResponseWithDataAsync(IRequest request, Type dataType, 
            IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default)
        {
            using var requestLogScope = BeginLogScope(request);
            LogRequest(request);
            
            var transportResponseContext = await ExecuteAsync(request, resiliencePolicy, cancellationToken)
                .ConfigureAwait(false);

            var response = await _responseBuilder
                .BuildAsync(request, transportResponseContext, allocateMemoryForContent: true, cancellationToken)
                .ConfigureAwait(false);
            LogResponse(response);
            
            if (_transportResponseMapper.CanMap(dataType, transportResponseContext))
                return BuildResponseWithData(await _transportResponseMapper
                    .MapAsync(dataType, transportResponseContext, cancellationToken)
                    .ConfigureAwait(false), dataType, response);
            
            var responseContext = new ResponseContext<IRequest, IResponse>(request, response);
            
            if (_responseMapper.CanMap(dataType, responseContext))
                return BuildResponseWithData(await _responseMapper
                    .MapAsync(dataType, responseContext, cancellationToken)
                    .ConfigureAwait(false), dataType, response);

            if (!_responseValidator.IsSuccess(transportResponseContext))
                return BuildResponseWithData(data: null, dataType, response);

            var dataObject = await TryGetDataObject(dataType, response.Content, cancellationToken).ConfigureAwait(false);
            return BuildResponseWithData(dataObject, dataType, response);
        }
        
        public async Task<IResponse> GetResponseWithErrorAsync(IRequest request, Type errorType, 
            IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default)
        {
            using var requestLogScope = BeginLogScope(request);
            LogRequest(request);
            
            var transportResponseContext = await ExecuteAsync(request, resiliencePolicy, cancellationToken)
                .ConfigureAwait(false);
            
            var response = await _responseBuilder
                .BuildAsync(request, transportResponseContext, allocateMemoryForContent: true, cancellationToken)
                .ConfigureAwait(false);
            LogResponse(response);
            
            if (_transportResponseMapper.CanMap(errorType, transportResponseContext))
                return BuildResponseWithError(await _transportResponseMapper
                    .MapAsync(errorType, transportResponseContext, cancellationToken)
                    .ConfigureAwait(false), errorType, response);
            
            var responseContext = new ResponseContext<IRequest, IResponse>(request, response);
            
            if (_responseMapper.CanMap(errorType, responseContext))
                return BuildResponseWithError(await _responseMapper
                    .MapAsync(errorType, responseContext, cancellationToken)
                    .ConfigureAwait(false), errorType, response);

            if (_responseValidator.IsSuccess(transportResponseContext))
                return BuildResponseWithError(error: null, errorType, response);
            
            var errorObject = await TryGetErrorObject(errorType, response.Content, cancellationToken).ConfigureAwait(false);
            return BuildResponseWithError(errorObject, errorType, response);
        }
        
        public async Task<IResponse> GetResponseWithDataOrErrorAsync(IRequest request, Type dataType, Type errorType, 
            IResiliencePolicy<TRequest, TResponse>? resiliencePolicy = null, CancellationToken cancellationToken = default)
        {
            using var requestLogScope = BeginLogScope(request);
            LogRequest(request);
            
            var transportResponseContext = await ExecuteAsync(request, resiliencePolicy, cancellationToken)
                .ConfigureAwait(false);
            
            var response = await _responseBuilder
                .BuildAsync(request, transportResponseContext, allocateMemoryForContent: true, cancellationToken)
                .ConfigureAwait(false);
            LogResponse(response);
            
            var responseContext = new ResponseContext<IRequest, IResponse>(request, response);

            if (!_responseValidator.IsSuccess(transportResponseContext))
            {
                if (_transportResponseMapper.CanMap(errorType, transportResponseContext))
                    return BuildResponseWithDataOrError(data: null, dataType, error: await _transportResponseMapper
                        .MapAsync(errorType, transportResponseContext, cancellationToken)
                        .ConfigureAwait(false), errorType, response);
                
                if (_responseMapper.CanMap(errorType, responseContext))
                    return BuildResponseWithDataOrError(data: null, dataType, error: await _responseMapper
                        .MapAsync(errorType, responseContext, cancellationToken)
                        .ConfigureAwait(false), errorType, response);
                
                var errorObject = await TryGetErrorObject(errorType, response.Content, cancellationToken).ConfigureAwait(false);
                return BuildResponseWithDataOrError(data: null, dataType, errorObject, errorType, response);
            }
            
            if (_transportResponseMapper.CanMap(dataType, transportResponseContext))
                return BuildResponseWithDataOrError(data: await _transportResponseMapper
                    .MapAsync(dataType, transportResponseContext, cancellationToken)
                    .ConfigureAwait(false), dataType, error: null, errorType, response);
            
            if (_responseMapper.CanMap(dataType, responseContext))
                return BuildResponseWithDataOrError(data: await _responseMapper
                    .MapAsync(dataType, responseContext, cancellationToken)
                    .ConfigureAwait(false), dataType, error: null, errorType, response);
            
            var dataObject = await TryGetDataObject(dataType, response.Content, cancellationToken).ConfigureAwait(false);
            return BuildResponseWithDataOrError(dataObject, dataType, error: null, errorType, response);
        }

        private async Task<IResponseContext<TRequest, TResponse>> ExecuteAsync(IRequest request, 
            IResiliencePolicy<TRequest, TResponse>? resiliencePolicy, CancellationToken cancellationToken = default)
        {
            return await (resiliencePolicy ?? _resiliencePolicy)
                .ExecuteAsync(token => ExecuteAttemptAsync(request, token), cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task<IResponseContext<TRequest, TResponse>> ExecuteAttemptAsync(IRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Start sending transport request");

            TRequest? transportRequest;
            TResponse? transportResponse;
            try
            {
                _logger.LogDebug("Start sending transport request attempt");
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
                
                _logger.LogDebug("Transport request attempt finished successfully");
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Transport request attempt failed with exception");
                throw;
            }
            
            _logger.LogInformation("Transport response received");
            return new ResponseContext<TRequest, TResponse>(transportRequest, transportResponse);
        }

        private async Task<object?> TryGetDataObject(Type dataType, IContent content, CancellationToken cancellationToken)
        {
            var stringContent = await content.Stream
                .ReadToEndAsync(content.Encoding, cancellationToken)
                .ConfigureAwait(false);
            
            return _serializer.Deserialize(stringContent, dataType);
        }

        private async Task<object?> TryGetErrorObject(Type errorType, IContent content, CancellationToken cancellationToken)
        {
            var stringContent = await content.Stream    
                .ReadToEndAsync(content.Encoding, cancellationToken)
                .ConfigureAwait(false);
            
            return _serializer.Deserialize(stringContent, errorType);
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

        private IDisposable BeginLogScope(IRequest request)
        {
            return _logger.BeginScope(new Dictionary<string, object>
            {
                ["RequestType"] = request.Type,
                ["RequestResource"] = request.Resource
            });
        }

        private void LogRequest(IRequest request)
        {
            _logger.LogInformation("Start sending {RequestType} request to {RequestResource}", request.Type, request.Resource);
        }
        
        private void LogResponse(IResponse response)
        {
            _logger.LogInformation("Response from {ResponseResource} received {ResponseStatusCode}", response.Resource, response.StatusCode);
        }
        
        private void LogTransportResponse(IRequest request)
        {
            _logger.LogInformation("Response from {ResponseResource} received", request.Resource);
        }
    }
}
