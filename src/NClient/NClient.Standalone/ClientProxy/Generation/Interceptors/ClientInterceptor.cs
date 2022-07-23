﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Annotations;
using NClient.Core.Helpers;
using NClient.Exceptions;
using NClient.Providers;
using NClient.Providers.Api;
using NClient.Providers.Caching;
using NClient.Providers.Authorization;
using NClient.Providers.Resilience;
using NClient.Providers.Transport;
using NClient.Standalone.Client;
using NClient.Standalone.ClientProxy.Generation.Helpers;
using NClient.Standalone.ClientProxy.Generation.Invocation;
using NClient.Standalone.ClientProxy.Generation.MethodBuilders;
using NClient.Standalone.Exceptions.Factories;
using AsyncInterceptorBase = NClient.Core.Castle.AsyncInterceptorBase;

namespace NClient.Standalone.ClientProxy.Generation.Interceptors
{
    internal class ClientInterceptor<TClient, TRequest, TResponse> : AsyncInterceptorBase
    {
        private readonly Uri _host;
        private readonly ITimeoutSelector _timeoutSelector;
        private readonly IGuidProvider _guidProvider;
        private readonly IMethodBuilder _methodBuilder;
        private readonly IExplicitMethodInvocationProvider<TRequest, TResponse> _explicitMethodInvocationProvider;
        private readonly IClientMethodInvocationProvider<TRequest, TResponse> _clientMethodInvocationProvider;
        private readonly IAuthorizationProvider _authorizationProvider;
        private readonly IRequestBuilderProvider _requestBuilderProvider;
        private readonly ITransportNClientFactory<TRequest, TResponse> _transportNClientFactory;
        private readonly IMethodResiliencePolicyProvider<TRequest, TResponse> _methodResiliencePolicyProvider;
        private readonly IClientRequestExceptionFactory _clientRequestExceptionFactory;
        private readonly IResponseCacheWorker? _responseCacheWorker;
        private readonly TimeSpan? _timeout;
        private readonly IToolset _toolset;
        
        public ClientInterceptor(
            Uri host,
            ITimeoutSelector timeoutSelector,
            IGuidProvider guidProvider,
            IMethodBuilder methodBuilder,
            IExplicitMethodInvocationProvider<TRequest, TResponse> explicitMethodInvocationProvider,
            IClientMethodInvocationProvider<TRequest, TResponse> clientMethodInvocationProvider,
            IAuthorizationProvider authorizationProvider,
            IRequestBuilderProvider requestBuilderProvider,
            ITransportNClientFactory<TRequest, TResponse> transportNClientFactory,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            IClientRequestExceptionFactory clientRequestExceptionFactory, 
            IResponseCacheWorker? responseCacheWorker,
            TimeSpan? timeout,
            IToolset toolset)
        {
            _host = host;
            _timeoutSelector = timeoutSelector;
            _guidProvider = guidProvider;
            _methodBuilder = methodBuilder;
            _explicitMethodInvocationProvider = explicitMethodInvocationProvider;
            _clientMethodInvocationProvider = clientMethodInvocationProvider;
            _authorizationProvider = authorizationProvider;
            _requestBuilderProvider = requestBuilderProvider;
            _transportNClientFactory = transportNClientFactory;
            _methodResiliencePolicyProvider = methodResiliencePolicyProvider;
            _clientRequestExceptionFactory = clientRequestExceptionFactory;
            _responseCacheWorker = responseCacheWorker;
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
            using var processLogScope = _toolset.Logger?.BeginScope(new Dictionary<string, object>
            {
                ["RequestId"] = requestId
            });
            _toolset.Logger?.LogDebug("Processing request started");
            
            ClientMethodInvocation<TRequest, TResponse>? methodInvocation = null;
            try
            {
                var transportNClient = _transportNClientFactory.Create();
                
                var explicitInvocation = _explicitMethodInvocationProvider.Get(typeof(TClient), invocation, resultType);
                var method = _methodBuilder.Build(typeof(TClient), explicitInvocation.Method, explicitInvocation.ReturnType);
                methodInvocation = _clientMethodInvocationProvider.Get(method, explicitInvocation);
                
                TimeSpan? TryGetFromMilliseconds(double? milliseconds)
                {
                    return milliseconds.HasValue
                        ? TimeSpan.FromMilliseconds(milliseconds.Value)
                        : null;
                }

                var authorization = _authorizationProvider.Create(_toolset);
                var timeout = _timeoutSelector.Get(transportNClient.Timeout, _timeout, TryGetFromMilliseconds(method.TimeoutAttribute?.Milliseconds));
                
                var cancellationToken = methodInvocation.CancellationToken ?? CancellationToken.None;
                using var timeoutCancellationTokenSource = new CancellationTokenSource(timeout);
                using var combinedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCancellationTokenSource.Token);
                var combinedCancellationToken = combinedCancellationTokenSource.Token;
                combinedCancellationToken.ThrowIfCancellationRequested();
                
                var request = await _requestBuilderProvider
                    .Create(_toolset)
                    .BuildAsync(requestId, _host, authorization, methodInvocation, combinedCancellationToken)
                    .ConfigureAwait(false);
                
                using var requestLogScope = _toolset.Logger?.BeginScope(new Dictionary<string, object>
                {
                    ["RequestResource"] = request.Resource,
                    ["RequestType"] = request.Type
                });
                
                var resiliencePolicy = methodInvocation.ResiliencePolicyProvider?.Create(_toolset)
                    ?? _methodResiliencePolicyProvider.Create(methodInvocation.Method, request, _toolset);

                var result = await ExecuteHttpResponseAsync(transportNClient, request, resultType, resiliencePolicy, method.CachingAttribute, combinedCancellationToken).ConfigureAwait(false);
                _toolset.Logger?.LogDebug("Processing request finished");
                return result;
            }
            catch (ClientValidationException e)
            {
                _toolset.Logger?.LogError(e, "Client validation error");
                e.InterfaceType = typeof(TClient);
                e.MethodInfo = invocation.Method;
                throw;
            }
            catch (ClientArgumentException e)
            {
                _toolset.Logger?.LogError(e, "Method call error");
                e.InterfaceType = typeof(TClient);
                e.MethodInfo = invocation.Method;
                throw;
            }
            catch (TransportException<TRequest, TResponse> e)
            {
                _toolset.Logger?.LogError(e, "Processing request error");
                throw _clientRequestExceptionFactory.WrapException(interfaceType: typeof(TClient), methodInvocation!.Method.Info, e);
            }
            catch (OperationCanceledException e)
            {
                _toolset.Logger?.LogWarning(e, "The request was canceled");
                throw;
            }
            catch (Exception e)
            {
                _toolset.Logger?.LogError(e, "Unexpected processing request error");
                throw _clientRequestExceptionFactory.WrapException(interfaceType: typeof(TClient), invocation.Method, e);
            }
        }
        
        private async Task<object?> ExecuteHttpResponseAsync(ITransportNClient<TRequest, TResponse> transportNClient, IRequest request, Type resultType, IResiliencePolicy<TRequest, TResponse>? resiliencePolicy, ICachingAttribute? cachingAttribute = default, CancellationToken cancellationToken = default)
        {
            if (await _responseCacheWorker.TryGet(request, cancellationToken) is { } cachedResult)
            {
                _toolset.Logger?.LogDebug("Response received from cache. Request id: '{requestId}'.", request.Id);
                return cachedResult;
            }
            
            if (resultType == typeof(TResponse))
                return await transportNClient
                    .GetTransportResponseAsync(request, resiliencePolicy, cancellationToken)
                    .ConfigureAwait(false);

            if (resultType == typeof(IResponse))
            {
                var result = await transportNClient
                    .GetResponseAsync(request, resiliencePolicy, cancellationToken)
                    .ConfigureAwait(false);
                
                await _responseCacheWorker.Put(request, result, cachingAttribute, cancellationToken);
                return result;
            }

            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(IResponse<>).GetGenericTypeDefinition())
            {
                var result = await transportNClient
                    .GetResponseWithDataAsync(request, dataType: resultType.GetGenericArguments().Single(), resiliencePolicy, cancellationToken)
                    .ConfigureAwait(false);
                
                await _responseCacheWorker.Put(request, result, cachingAttribute, cancellationToken);
                return result;
            }

            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(IResponseWithError<>).GetGenericTypeDefinition())
            {
                var result = await transportNClient
                    .GetResponseWithErrorAsync(request, errorType: resultType.GetGenericArguments().Single(), resiliencePolicy, cancellationToken)
                    .ConfigureAwait(false);
                
                await _responseCacheWorker.Put(request, result, cachingAttribute, cancellationToken);
                return result;
            }

            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(IResponseWithError<,>).GetGenericTypeDefinition())
            {
                var result = await transportNClient
                    .GetResponseWithDataOrErrorAsync(request, dataType: resultType.GetGenericArguments()[0], errorType: resultType.GetGenericArguments()[1], resiliencePolicy, cancellationToken)
                    .ConfigureAwait(false);
                
                await _responseCacheWorker.Put(request, result, cachingAttribute, cancellationToken);
                return result;
            }

            if (resultType != typeof(void))
                return await transportNClient
                    .GetResultAsync(request, resultType, resiliencePolicy, cancellationToken)
                    .ConfigureAwait(false);
            
            await transportNClient
                .GetResultAsync(request, resiliencePolicy, cancellationToken)
                .ConfigureAwait(false);
            
            return null;
        }
    }
}
