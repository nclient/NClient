using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Exceptions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Mapping;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Core.Helpers;
using NClient.Exceptions;
using NClient.Providers.Results.HttpMessages;
using NClient.Standalone.Exceptions.Factories;
using NClient.Standalone.Interceptors.HttpClients;
using NClient.Standalone.Interceptors.Invocation;
using NClient.Standalone.Interceptors.MethodBuilders;
using NClient.Standalone.Interceptors.RequestBuilders;
using NClient.Standalone.Interceptors.Validation;
using AsyncInterceptorBase = NClient.Core.Castle.AsyncInterceptorBase;

namespace NClient.Standalone.Interceptors
{
    internal class ClientInterceptor<TClient, TRequest, TResponse> : AsyncInterceptorBase
    {
        private readonly Uri _host;
        private readonly IHttpMessageBuilder<TRequest, TResponse> _httpMessageBuilder;
        private readonly IResilienceHttpClientProvider<TRequest, TResponse> _resilienceHttpClientProvider;
        private readonly IFullMethodInvocationProvider<TRequest, TResponse> _fullMethodInvocationProvider;
        private readonly ISerializerProvider _serializerProvider;
        private readonly IResponseValidator<TRequest, TResponse> _responseValidator;
        private readonly IReadOnlyCollection<IResponseMapper> _responseMappers;
        private readonly IClientRequestExceptionFactory<TResponse> _clientRequestExceptionFactory;
        private readonly IMethodBuilder _methodBuilder;
        private readonly IRequestBuilder _requestBuilder;
        private readonly IGuidProvider _guidProvider;
        private readonly ILogger<TClient>? _logger;

        public ClientInterceptor(
            Uri host,
            IHttpMessageBuilder<TRequest, TResponse> httpMessageBuilder,
            IResilienceHttpClientProvider<TRequest, TResponse> resilienceHttpClientProvider,
            IFullMethodInvocationProvider<TRequest, TResponse> fullMethodInvocationProvider,
            ISerializerProvider serializerProvider,
            IEnumerable<IResponseMapper> responseMappers,
            IResponseValidator<TRequest, TResponse> responseValidator,
            IClientRequestExceptionFactory<TResponse> clientRequestExceptionFactory,
            IMethodBuilder methodBuilder,
            IRequestBuilder requestBuilder,
            IGuidProvider guidProvider,
            ILogger<TClient>? logger = null)
        {
            _host = host;
            _httpMessageBuilder = httpMessageBuilder;
            _resilienceHttpClientProvider = resilienceHttpClientProvider;
            _fullMethodInvocationProvider = fullMethodInvocationProvider;
            _serializerProvider = serializerProvider;
            _responseValidator = responseValidator;
            _responseMappers = responseMappers.ToArray();
            _clientRequestExceptionFactory = clientRequestExceptionFactory;
            _methodBuilder = methodBuilder;
            _requestBuilder = requestBuilder;
            _guidProvider = guidProvider;
            _logger = logger;
        }

        protected override async Task InterceptAsync(
            IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> _)
        {
            await ProcessInvocationAsync(invocation, resultType: typeof(void))
                .ConfigureAwait(false);
        }

        protected override async Task<TResult> InterceptAsync<TResult>(
            IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> _)
        {
            return (TResult)await ProcessInvocationAsync(invocation, resultType: typeof(TResult)).ConfigureAwait(false);
        }

        private async Task<object?> ProcessInvocationAsync(IInvocation invocation, Type resultType)
        {
            var requestId = _guidProvider.Create();
            using var loggingScope = _logger?.BeginScope("Processing request {requestId}.", requestId);
            
            IResponseContext<TRequest, TResponse>? responseContext = default;
            try
            {
                var fullMethodInvocation = _fullMethodInvocationProvider
                    .Get(interfaceType: typeof(TClient), resultType, invocation);
                var clientMethod = _methodBuilder
                    .Build(fullMethodInvocation.ClientType, fullMethodInvocation.MethodInfo);

                var httpRequest = _requestBuilder.Build(requestId, _host, clientMethod, fullMethodInvocation.MethodArguments);
                
                try
                {
                    responseContext = await _resilienceHttpClientProvider
                        .Create(fullMethodInvocation.ResiliencePolicyProvider)
                        .ExecuteAsync(httpRequest, fullMethodInvocation)
                        .ConfigureAwait(false);
                }
                catch (HttpClientException<TRequest, TResponse> e)
                {
                    responseContext = new ResponseContext<TRequest, TResponse>(e.Request, e.Response, fullMethodInvocation);
                    throw;
                }
                finally
                {
                    _logger?.LogDebug("Processing request finished. Request id: '{requestId}'.", requestId);
                }
                
                if (resultType == typeof(void))
                    return responseContext.Response;

                if (resultType == typeof(TResponse))
                    return responseContext.Response;

                var httpResponse = await _httpMessageBuilder
                    .BuildResponseAsync(httpRequest, responseContext.Response)
                    .ConfigureAwait(false);

                if (resultType == typeof(HttpResponse) || resultType == typeof(IHttpResponse))
                    return httpResponse;

                if (_responseMappers.FirstOrDefault(x => x.CanMapTo(resultType)) is { } responseMapper)
                    return await responseMapper
                        .MapAsync(resultType, httpResponse, _serializerProvider.Create())
                        .ConfigureAwait(false);
                
                _responseValidator.Ensure(responseContext);

                return _serializerProvider.Create().Deserialize(httpResponse.Content.ToString(), resultType);
            }
            catch (ClientValidationException e)
            {
                _logger?.LogError(e, "Client validation error. Request id: '{requestId}'.", requestId);
                e.InterfaceType = typeof(TClient);
                e.MethodInfo = invocation.Method;
                throw;
            }
            catch (ClientArgumentException e)
            {
                _logger?.LogError(e, "Method call error. Request id: '{requestId}'.", requestId);
                e.InterfaceType = typeof(TClient);
                e.MethodInfo = invocation.Method;
                throw;
            }
            catch (HttpClientException<TRequest, TResponse> e)
            {
                _logger?.LogError(e, "Processing request error. Request id: '{requestId}'.", requestId);
                throw _clientRequestExceptionFactory.WrapClientHttpRequestException(interfaceType: typeof(TClient), invocation.Method, responseContext!.Response, e);
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Unexpected processing request error. Request id: '{requestId}'.", requestId);
                if (responseContext is not null)
                    throw _clientRequestExceptionFactory.WrapException(interfaceType: typeof(TClient), invocation.Method, responseContext.Response, e);
                throw _clientRequestExceptionFactory.WrapException(interfaceType: typeof(TClient), invocation.Method, e);
            }
        }
    }
}
