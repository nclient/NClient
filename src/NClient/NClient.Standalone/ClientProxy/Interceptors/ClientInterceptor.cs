using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Exceptions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Core.Helpers;
using NClient.Exceptions;
using NClient.Standalone.Client;
using NClient.Standalone.ClientProxy.Interceptors.Invocation;
using NClient.Standalone.ClientProxy.Interceptors.MethodBuilders;
using NClient.Standalone.ClientProxy.Interceptors.RequestBuilders;
using NClient.Standalone.Exceptions.Factories;
using AsyncInterceptorBase = NClient.Core.Castle.AsyncInterceptorBase;

namespace NClient.Standalone.ClientProxy.Interceptors
{
    internal class ClientInterceptor<TClient, TRequest, TResponse> : AsyncInterceptorBase
    {
        private readonly Uri _host;
        private readonly IGuidProvider _guidProvider;
        private readonly IMethodBuilder _methodBuilder;
        private readonly IFullMethodInvocationProvider<TRequest, TResponse> _fullMethodInvocationProvider;
        private readonly IRequestBuilder _requestBuilder;
        private readonly IHttpNClientFactory<TRequest, TResponse> _httpNClientFactory;
        private readonly IMethodResiliencePolicyProvider<TRequest, TResponse> _methodResiliencePolicyProvider;
        private readonly IClientRequestExceptionFactory _clientRequestExceptionFactory;
        private readonly ILogger<TClient>? _logger;

        public ClientInterceptor(
            Uri host,
            IGuidProvider guidProvider,
            IMethodBuilder methodBuilder,
            IFullMethodInvocationProvider<TRequest, TResponse> fullMethodInvocationProvider,
            IRequestBuilder requestBuilder,
            IHttpNClientFactory<TRequest, TResponse> httpNClientFactory,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            IClientRequestExceptionFactory clientRequestExceptionFactory,
            ILogger<TClient>? logger = null)
        {
            _host = host;
            _guidProvider = guidProvider;
            _methodBuilder = methodBuilder;
            _fullMethodInvocationProvider = fullMethodInvocationProvider;
            _requestBuilder = requestBuilder;
            _httpNClientFactory = httpNClientFactory;
            _methodResiliencePolicyProvider = methodResiliencePolicyProvider;
            _clientRequestExceptionFactory = clientRequestExceptionFactory;
            _logger = logger;
        }

        protected override async Task InterceptAsync(
            IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> _)
        {
            await ProcessInvocationAsync(invocation, typeof(void)).ConfigureAwait(false);
        }

        protected override async Task<TResult> InterceptAsync<TResult>(
            IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> _)
        {
            #pragma warning disable 8600, 8603
            return (TResult)await ProcessInvocationAsync(invocation, typeof(TResult)).ConfigureAwait(false);
            #pragma warning restore 8600, 8603
        }

        private async Task<object?> ProcessInvocationAsync(IInvocation invocation, Type resultType)
        {
            var requestId = _guidProvider.Create();
            using var loggingScope = _logger?.BeginScope("Processing request {requestId}.", requestId);

            FullMethodInvocation<TRequest, TResponse>? fullMethodInvocation = null;
            IHttpRequest? httpRequest = null;
            try
            {
                fullMethodInvocation = _fullMethodInvocationProvider
                    .Get(interfaceType: typeof(TClient), resultType, invocation);
                var clientMethod = _methodBuilder
                    .Build(fullMethodInvocation.ClientType, fullMethodInvocation.MethodInfo);
                
                httpRequest = _requestBuilder.Build(requestId, _host, clientMethod, fullMethodInvocation.MethodArguments);
                var resiliencePolicy = fullMethodInvocation.ResiliencePolicyProvider?.Create()
                    ?? _methodResiliencePolicyProvider.Create(fullMethodInvocation.MethodInfo, httpRequest);
                var result = await ExecuteHttpResponseAsync(httpRequest, resultType, resiliencePolicy)
                    .ConfigureAwait(false);

                _logger?.LogDebug("Processing request finished. Request id: '{requestId}'.", requestId);
                return result;
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
                _logger?.LogError(e, "Processing request error. Request id: '{requestId}'.", httpRequest!.Id);
                throw _clientRequestExceptionFactory.WrapException(interfaceType: typeof(TClient), fullMethodInvocation!.MethodInfo, e);
            }
            catch (Exception e)
            {
                // TODO
                _logger?.LogError(e, "Unexpected processing request error. Request id: '{requestId}'.", requestId);
                throw _clientRequestExceptionFactory.WrapException(interfaceType: typeof(TClient), invocation.Method, e);
            }
        }
        
        private async Task<object?> ExecuteHttpResponseAsync(IHttpRequest httpRequest, Type resultType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy)
        {
            if (resultType == typeof(TResponse))
                return await _httpNClientFactory
                    .Create()
                    .GetOriginalResponseAsync(httpRequest, resiliencePolicy)
                    .ConfigureAwait(false);

            if (resultType == typeof(IHttpResponse))
                return await _httpNClientFactory
                    .Create()
                    .GetHttpResponseAsync(httpRequest, resiliencePolicy)
                    .ConfigureAwait(false);

            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(IHttpResponse<>).GetGenericTypeDefinition())
                return await _httpNClientFactory
                    .Create()
                    .GetHttpResponseAsync(httpRequest, dataType: resultType.GetGenericArguments().Single(), resiliencePolicy)
                    .ConfigureAwait(false);

            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(IHttpResponseWithError<>).GetGenericTypeDefinition())
                return await _httpNClientFactory
                    .Create()
                    .GetHttpResponseWithErrorAsync(httpRequest, errorType: resultType.GetGenericArguments().Single(), resiliencePolicy)
                    .ConfigureAwait(false);

            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(IHttpResponseWithError<,>).GetGenericTypeDefinition())
                return await _httpNClientFactory
                    .Create()
                    .GetHttpResponseWithDataAndErrorAsync(httpRequest, dataType: resultType.GetGenericArguments()[0], errorType: resultType.GetGenericArguments()[1], resiliencePolicy)
                    .ConfigureAwait(false);

            if (resultType != typeof(void))
                return await _httpNClientFactory
                    .Create()
                    .GetResultAsync(httpRequest, resultType, resiliencePolicy)
                    .ConfigureAwait(false);
            
            await _httpNClientFactory
                .Create()
                .GetResultAsync(httpRequest, resiliencePolicy)
                .ConfigureAwait(false);
            return null;
        }
    }
}
