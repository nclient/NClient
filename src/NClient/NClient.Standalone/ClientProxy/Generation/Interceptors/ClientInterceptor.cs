using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Core.Helpers;
using NClient.Exceptions;
using NClient.Providers.Api;
using NClient.Providers.Resilience;
using NClient.Providers.Transport;
using NClient.Standalone.Client;
using NClient.Standalone.ClientProxy.Generation.Invocation;
using NClient.Standalone.ClientProxy.Generation.MethodBuilders;
using NClient.Standalone.Exceptions.Factories;
using AsyncInterceptorBase = NClient.Core.Castle.AsyncInterceptorBase;

namespace NClient.Standalone.ClientProxy.Generation.Interceptors
{
    internal class ClientInterceptor<TClient, TRequest, TResponse> : AsyncInterceptorBase
    {
        private readonly string _resourceRoot;
        private readonly IGuidProvider _guidProvider;
        private readonly IMethodBuilder _methodBuilder;
        private readonly IExplicitInvocationProvider<TRequest, TResponse> _explicitInvocationProvider;
        private readonly IRequestBuilder _requestBuilder;
        private readonly ITransportNClientFactory<TRequest, TResponse> _transportNClientFactory;
        private readonly IMethodResiliencePolicyProvider<TRequest, TResponse> _methodResiliencePolicyProvider;
        private readonly IClientRequestExceptionFactory _clientRequestExceptionFactory;
        private readonly ILogger<TClient>? _logger;

        public ClientInterceptor(
            string resourceRoot,
            IGuidProvider guidProvider,
            IMethodBuilder methodBuilder,
            IExplicitInvocationProvider<TRequest, TResponse> explicitInvocationProvider,
            IRequestBuilder requestBuilder,
            ITransportNClientFactory<TRequest, TResponse> transportNClientFactory,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            IClientRequestExceptionFactory clientRequestExceptionFactory,
            ILogger<TClient>? logger = null)
        {
            _resourceRoot = resourceRoot;
            _guidProvider = guidProvider;
            _methodBuilder = methodBuilder;
            _explicitInvocationProvider = explicitInvocationProvider;
            _requestBuilder = requestBuilder;
            _transportNClientFactory = transportNClientFactory;
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

            FullMethodInvocation<TRequest, TResponse>? methodInvocation = null;
            IRequest? httpRequest = null;
            try
            {
                var explicitInvocation = _explicitInvocationProvider.Get(typeof(TClient), invocation, resultType);
                var method = _methodBuilder.Build(typeof(TClient), explicitInvocation.Method, explicitInvocation.ReturnType);
                methodInvocation = new FullMethodInvocation<TRequest, TResponse>(method, explicitInvocation);
                
                httpRequest = _requestBuilder.Build(requestId, _resourceRoot, methodInvocation);
                var resiliencePolicy = explicitInvocation.ResiliencePolicyProvider?.Create()
                    ?? _methodResiliencePolicyProvider.Create(methodInvocation.Method.Info, httpRequest);
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
            catch (TransportException<TRequest, TResponse> e)
            {
                _logger?.LogError(e, "Processing request error. Request id: '{requestId}'.", httpRequest!.Id);
                throw _clientRequestExceptionFactory.WrapException(interfaceType: typeof(TClient), methodInvocation!.Method.Info, e);
            }
            catch (Exception e)
            {
                // TODO
                _logger?.LogError(e, "Unexpected processing request error. Request id: '{requestId}'.", requestId);
                throw _clientRequestExceptionFactory.WrapException(interfaceType: typeof(TClient), invocation.Method, e);
            }
        }
        
        private async Task<object?> ExecuteHttpResponseAsync(IRequest request, Type resultType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy)
        {
            if (resultType == typeof(TResponse))
                return await _transportNClientFactory
                    .Create()
                    .GetOriginalResponseAsync(request, resiliencePolicy)
                    .ConfigureAwait(false);

            if (resultType == typeof(IResponse))
                return await _transportNClientFactory
                    .Create()
                    .GetHttpResponseAsync(request, resiliencePolicy)
                    .ConfigureAwait(false);

            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(IResponse<>).GetGenericTypeDefinition())
                return await _transportNClientFactory
                    .Create()
                    .GetHttpResponseAsync(request, dataType: resultType.GetGenericArguments().Single(), resiliencePolicy)
                    .ConfigureAwait(false);

            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(IResponseWithError<>).GetGenericTypeDefinition())
                return await _transportNClientFactory
                    .Create()
                    .GetHttpResponseWithErrorAsync(request, errorType: resultType.GetGenericArguments().Single(), resiliencePolicy)
                    .ConfigureAwait(false);

            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(IResponseWithError<,>).GetGenericTypeDefinition())
                return await _transportNClientFactory
                    .Create()
                    .GetHttpResponseWithDataAndErrorAsync(request, dataType: resultType.GetGenericArguments()[0], errorType: resultType.GetGenericArguments()[1], resiliencePolicy)
                    .ConfigureAwait(false);

            if (resultType != typeof(void))
                return await _transportNClientFactory
                    .Create()
                    .GetResultAsync(request, resultType, resiliencePolicy)
                    .ConfigureAwait(false);
            
            await _transportNClientFactory
                .Create()
                .GetResultAsync(request, resiliencePolicy)
                .ConfigureAwait(false);
            return null;
        }
    }
}
