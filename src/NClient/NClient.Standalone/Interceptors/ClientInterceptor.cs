using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Exceptions;
using NClient.Abstractions.HttpClients;
using NClient.Core.Helpers;
using NClient.Exceptions;
using NClient.Standalone.Exceptions.Factories;
using NClient.Standalone.Interceptors.HttpClients;
using NClient.Standalone.Interceptors.HttpResponsePopulation;
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
        private readonly IHttpMessageBuilder<TRequest, TResponse> _httpMessageBuilder;
        private readonly IHttpResponsePopulater _httpResponsePopulater;
        private readonly IClientRequestExceptionFactory _clientRequestExceptionFactory;
        private readonly IMethodBuilder _methodBuilder;
        private readonly IRequestBuilder _requestBuilder;
        private readonly IGuidProvider _guidProvider;
        private readonly ILogger<TClient>? _logger;

        public ClientInterceptor(
            Uri host,
            IResilienceHttpClientProvider<TRequest, TResponse> resilienceHttpClientProvider,
            IFullMethodInvocationProvider<TRequest, TResponse> fullMethodInvocationProvider,
            IHttpMessageBuilder<TRequest, TResponse> httpMessageBuilder,
            IHttpResponsePopulater httpResponsePopulater,
            IClientRequestExceptionFactory clientRequestExceptionFactory,
            IMethodBuilder methodBuilder,
            IRequestBuilder requestBuilder,
            IGuidProvider guidProvider,
            ILogger<TClient>? logger = null)
        {
            _host = host;
            _resilienceHttpClientProvider = resilienceHttpClientProvider;
            _fullMethodInvocationProvider = fullMethodInvocationProvider;
            _httpMessageBuilder = httpMessageBuilder;
            _httpResponsePopulater = httpResponsePopulater;
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
            return (TResult)await ProcessInvocationAsync(invocation, resultType: typeof(TResult))
                .ConfigureAwait(false);
        }

        private async Task<object> ProcessInvocationAsync(IInvocation invocation, Type resultType)
        {
            var requestId = _guidProvider.Create();
            using var loggingScope = _logger?.BeginScope("Processing request {requestId}.", requestId);
            
            IHttpResponse? httpResponse = null;
            IHttpResponse? populatedHttpResponse = null;
            try
            {
                var fullMethodInvocation = _fullMethodInvocationProvider
                    .Get(interfaceType: typeof(TClient), resultType, invocation);
                var clientMethod = _methodBuilder
                    .Build(fullMethodInvocation.ClientType, fullMethodInvocation.MethodInfo);

                var httpRequest = _requestBuilder.Build(requestId, _host, clientMethod, fullMethodInvocation.MethodArguments);

                TResponse? response = default;
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
                    if (response is not null)
                    {
                        httpResponse = await _httpMessageBuilder
                            .BuildResponseAsync(httpRequest!, response)
                            .ConfigureAwait(false);
                        populatedHttpResponse = _httpResponsePopulater.Populate(httpResponse, resultType);   
                    }
                    
                    _logger?.LogDebug("Processing request finished. Request id: '{requestId}'.", requestId);
                }

                if (resultType == typeof(TResponse))
                    return response!;

                if (typeof(IHttpResponse).IsAssignableFrom(resultType))
                    return populatedHttpResponse!;

                return populatedHttpResponse!
                    .GetType()
                    .GetProperty(nameof(IHttpResponse<object>.Value))?
                    .GetValue(populatedHttpResponse)!;
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
                throw _clientRequestExceptionFactory.WrapClientHttpRequestException(interfaceType: typeof(TClient), invocation.Method, populatedHttpResponse!, e);
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Unexpected processing request error. Request id: '{requestId}'.", requestId);
                if (populatedHttpResponse is not null)
                    throw _clientRequestExceptionFactory.WrapException(interfaceType: typeof(TClient), invocation.Method, populatedHttpResponse, e);
                if (httpResponse is not null)
                    throw _clientRequestExceptionFactory.WrapException(interfaceType: typeof(TClient), invocation.Method, httpResponse, e);
                throw _clientRequestExceptionFactory.WrapException(interfaceType: typeof(TClient), invocation.Method, e);
            }
        }
    }
}
