using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Exceptions;
using NClient.Abstractions.Mapping;
using NClient.Abstractions.Serialization;
using NClient.Core.Helpers;
using NClient.Exceptions;
using NClient.Standalone.Exceptions.Factories;
using NClient.Standalone.Interceptors.HttpClients;
using NClient.Standalone.Interceptors.Invocation;
using NClient.Standalone.Interceptors.MethodBuilders;
using NClient.Standalone.Interceptors.RequestBuilders;
using AsyncInterceptorBase = NClient.Core.Castle.AsyncInterceptorBase;

namespace NClient.Standalone.Interceptors
{
    internal class ClientInterceptor<TClient, TRequest, TResponse> : AsyncInterceptorBase
    {
        private readonly Uri _host;
        private readonly IResilienceHttpClientProvider<TRequest, TResponse> _resilienceHttpClientProvider;
        private readonly IFullMethodInvocationProvider<TRequest, TResponse> _fullMethodInvocationProvider;
        private readonly ISerializerProvider _serializerProvider;
        private readonly IReadOnlyCollection<IResponseMapper<TResponse>> _responseMappers;
        private readonly IClientRequestExceptionFactory<TResponse> _clientRequestExceptionFactory;
        private readonly IMethodBuilder _methodBuilder;
        private readonly IRequestBuilder _requestBuilder;
        private readonly IGuidProvider _guidProvider;
        private readonly ILogger<TClient>? _logger;

        public ClientInterceptor(
            Uri host,
            IResilienceHttpClientProvider<TRequest, TResponse> resilienceHttpClientProvider,
            IFullMethodInvocationProvider<TRequest, TResponse> fullMethodInvocationProvider,
            ISerializerProvider serializerProvider,
            IEnumerable<IResponseMapper<TResponse>> responseMappers,
            IClientRequestExceptionFactory<TResponse> clientRequestExceptionFactory,
            IMethodBuilder methodBuilder,
            IRequestBuilder requestBuilder,
            IGuidProvider guidProvider,
            ILogger<TClient>? logger = null)
        {
            _host = host;
            _resilienceHttpClientProvider = resilienceHttpClientProvider;
            _fullMethodInvocationProvider = fullMethodInvocationProvider;
            _serializerProvider = serializerProvider;
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
            
            TResponse? response = default;
            try
            {
                var fullMethodInvocation = _fullMethodInvocationProvider
                    .Get(interfaceType: typeof(TClient), resultType, invocation);
                var clientMethod = _methodBuilder
                    .Build(fullMethodInvocation.ClientType, fullMethodInvocation.MethodInfo);

                var httpRequest = _requestBuilder.Build(requestId, _host, clientMethod, fullMethodInvocation.MethodArguments);
                
                try
                {
                    response = await _resilienceHttpClientProvider
                        .Create(fullMethodInvocation.ResiliencePolicyProvider)
                        .ExecuteAsync(httpRequest, fullMethodInvocation)
                        .ConfigureAwait(false);
                }
                catch (HttpClientException<TRequest, TResponse> e)
                {
                    response = e.Response;
                    throw;
                }
                finally
                {
                    _logger?.LogDebug("Processing request finished. Request id: '{requestId}'.", requestId);
                }
                if (resultType == typeof(void))
                    return response;

                if (resultType == typeof(TResponse))
                    return response!;

                if (_responseMappers.FirstOrDefault(x => x.CanMapTo(resultType)) is { } responseMapper)
                    return await responseMapper
                        .MapAsync(resultType, httpRequest, response, _serializerProvider.Create())
                        .ConfigureAwait(false);

                // TODO: use specific exception
                throw new Exception();
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
                throw _clientRequestExceptionFactory.WrapClientHttpRequestException(interfaceType: typeof(TClient), invocation.Method, response!, e);
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Unexpected processing request error. Request id: '{requestId}'.", requestId);
                if (response is not null)
                    throw _clientRequestExceptionFactory.WrapException(interfaceType: typeof(TClient), invocation.Method, response, e);
                throw _clientRequestExceptionFactory.WrapException(interfaceType: typeof(TClient), invocation.Method, e);
            }
        }
    }
}
