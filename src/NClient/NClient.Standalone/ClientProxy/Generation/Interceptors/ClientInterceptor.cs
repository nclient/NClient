using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Core.Helpers;
using NClient.Exceptions;
using NClient.Providers;
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
        private readonly string _resource;
        private readonly IGuidProvider _guidProvider;
        private readonly IMethodBuilder _methodBuilder;
        private readonly IExplicitMethodInvocationProvider<TRequest, TResponse> _explicitMethodInvocationProvider;
        private readonly IClientMethodInvocationProvider<TRequest, TResponse> _clientMethodInvocationProvider;
        private readonly IRequestBuilder _requestBuilder;
        private readonly ITransportNClientFactory<TRequest, TResponse> _transportNClientFactory;
        private readonly IMethodResiliencePolicyProvider<TRequest, TResponse> _methodResiliencePolicyProvider;
        private readonly IClientRequestExceptionFactory _clientRequestExceptionFactory;
        private readonly TimeSpan? _timeout;
        private readonly IToolset _toolset;

        public ClientInterceptor(
            string resource,
            IGuidProvider guidProvider,
            IMethodBuilder methodBuilder,
            IExplicitMethodInvocationProvider<TRequest, TResponse> explicitMethodInvocationProvider,
            IClientMethodInvocationProvider<TRequest, TResponse> clientMethodInvocationProvider,
            IRequestBuilder requestBuilder,
            ITransportNClientFactory<TRequest, TResponse> transportNClientFactory,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            IClientRequestExceptionFactory clientRequestExceptionFactory,
            TimeSpan? timeout,
            IToolset toolset)
        {
            _resource = resource;
            _guidProvider = guidProvider;
            _methodBuilder = methodBuilder;
            _explicitMethodInvocationProvider = explicitMethodInvocationProvider;
            _clientMethodInvocationProvider = clientMethodInvocationProvider;
            _requestBuilder = requestBuilder;
            _transportNClientFactory = transportNClientFactory;
            _methodResiliencePolicyProvider = methodResiliencePolicyProvider;
            _clientRequestExceptionFactory = clientRequestExceptionFactory;
            _timeout = timeout;
            _toolset = toolset;
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
            return (TResult) await ProcessInvocationAsync(invocation, typeof(TResult)).ConfigureAwait(false);
            #pragma warning restore 8600, 8603
        }

        private async Task<object?> ProcessInvocationAsync(IInvocation invocation, Type resultType)
        {
            var requestId = _guidProvider.Create();
            using var loggingScope = _toolset.Logger?.BeginScope("Processing request {requestId}.", requestId);

            ClientMethodInvocation<TRequest, TResponse>? methodInvocation = null;
            IRequest? httpRequest = null;
            try
            {
                var explicitInvocation = _explicitMethodInvocationProvider.Get(typeof(TClient), invocation, resultType);
                var method = _methodBuilder.Build(typeof(TClient), explicitInvocation.Method, explicitInvocation.ReturnType);
                methodInvocation = _clientMethodInvocationProvider.Get(method, explicitInvocation);

                var cancellationToken = methodInvocation.CancellationToken ?? CancellationToken.None;
                using var timeoutCancellationTokenSource = method.TimeoutAttribute is null && _timeout is null 
                    ? new CancellationTokenSource() 
                    : new CancellationTokenSource(_timeout ?? TimeSpan.FromMilliseconds(method.TimeoutAttribute!.Milliseconds));
                using var combinedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCancellationTokenSource.Token);
                var combinedCancellationToken = combinedCancellationTokenSource.Token;
                combinedCancellationToken.ThrowIfCancellationRequested();
                    
                httpRequest = await _requestBuilder
                    .BuildAsync(requestId, _resource, methodInvocation, combinedCancellationToken)
                    .ConfigureAwait(false);
                
                var resiliencePolicy = methodInvocation.ResiliencePolicyProvider?.Create(_toolset)
                    ?? _methodResiliencePolicyProvider.Create(methodInvocation.Method, httpRequest, _toolset);

                var result = await ExecuteHttpResponseAsync(httpRequest, resultType, resiliencePolicy, combinedCancellationToken).ConfigureAwait(false);
                _toolset.Logger?.LogDebug("Processing request finished. Request id: '{requestId}'.", requestId);
                return result;
            }
            catch (ClientValidationException e)
            {
                _toolset.Logger?.LogError(e, "Client validation error. Request id: '{requestId}'.", requestId);
                e.InterfaceType = typeof(TClient);
                e.MethodInfo = invocation.Method;
                throw;
            }
            catch (ClientArgumentException e)
            {
                _toolset.Logger?.LogError(e, "Method call error. Request id: '{requestId}'.", requestId);
                e.InterfaceType = typeof(TClient);
                e.MethodInfo = invocation.Method;
                throw;
            }
            catch (TransportException<TRequest, TResponse> e)
            {
                _toolset.Logger?.LogError(e, "Processing request error. Request id: '{requestId}'.", httpRequest!.Id);
                throw _clientRequestExceptionFactory.WrapException(interfaceType: typeof(TClient), methodInvocation!.Method.Info, e);
            }
            catch (Exception e) when (e is TaskCanceledException or OperationCanceledException)
            {
                _toolset.Logger?.LogWarning(e, "The request was canceled. Request id: '{requestId}'.", requestId);
                throw;
            }
            catch (Exception e)
            {
                _toolset.Logger?.LogError(e, "Unexpected processing request error. Request id: '{requestId}'.", requestId);
                throw _clientRequestExceptionFactory.WrapException(interfaceType: typeof(TClient), invocation.Method, e);
            }
        }
        
        private async Task<object?> ExecuteHttpResponseAsync(IRequest request, Type resultType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy, CancellationToken cancellationToken)
        {
            if (resultType == typeof(TResponse))
                return await _transportNClientFactory
                    .Create()
                    .GetOriginalResponseAsync(request, resiliencePolicy, cancellationToken)
                    .ConfigureAwait(false);

            if (resultType == typeof(IResponse))
                return await _transportNClientFactory
                    .Create()
                    .GetHttpResponseAsync(request, resiliencePolicy, cancellationToken)
                    .ConfigureAwait(false);

            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(IResponse<>).GetGenericTypeDefinition())
                return await _transportNClientFactory
                    .Create()
                    .GetHttpResponseAsync(request, dataType: resultType.GetGenericArguments().Single(), resiliencePolicy, cancellationToken)
                    .ConfigureAwait(false);

            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(IResponseWithError<>).GetGenericTypeDefinition())
                return await _transportNClientFactory
                    .Create()
                    .GetHttpResponseWithErrorAsync(request, errorType: resultType.GetGenericArguments().Single(), resiliencePolicy, cancellationToken)
                    .ConfigureAwait(false);

            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(IResponseWithError<,>).GetGenericTypeDefinition())
                return await _transportNClientFactory
                    .Create()
                    .GetHttpResponseWithDataAndErrorAsync(request, dataType: resultType.GetGenericArguments()[0], errorType: resultType.GetGenericArguments()[1], resiliencePolicy, cancellationToken)
                    .ConfigureAwait(false);

            if (resultType != typeof(void))
                return await _transportNClientFactory
                    .Create()
                    .GetResultAsync(request, resultType, resiliencePolicy, cancellationToken)
                    .ConfigureAwait(false);
            
            await _transportNClientFactory
                .Create()
                .GetResultAsync(request, resiliencePolicy, cancellationToken)
                .ConfigureAwait(false);
            return null;
        }
    }
}
