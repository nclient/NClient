using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Exceptions;
using NClient.Abstractions.HttpClients;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers;
using NClient.Core.Interceptors.ClientInvocations;
using NClient.Core.Interceptors.HttpClients;
using NClient.Core.Interceptors.HttpResponsePopulation;
using NClient.Core.Interceptors.MethodBuilders;
using NClient.Core.Interceptors.RequestBuilders;
using NClient.Exceptions;
using AsyncInterceptorBase = NClient.Core.Castle.AsyncInterceptorBase;

namespace NClient.Core.Interceptors
{
    internal class ClientInterceptor<T> : AsyncInterceptorBase
    {
        private readonly Uri _host;
        private readonly IResilienceHttpClientProvider _resilienceHttpClientProvider;
        private readonly IClientInvocationProvider _clientInvocationProvider;
        private readonly IClientRequestExceptionFactory _clientRequestExceptionFactory;
        private readonly IMethodBuilder _methodBuilder;
        private readonly IRequestBuilder _requestBuilder;
        private readonly IHttpResponsePopulater _httpResponsePopulater;
        private readonly IGuidProvider _guidProvider;
        private readonly Type? _controllerType;
        private readonly ILogger<T>? _logger;

        public ClientInterceptor(
            Uri host,
            IResilienceHttpClientProvider resilienceHttpClientProvider,
            IClientInvocationProvider clientInvocationProvider,
            IClientRequestExceptionFactory clientRequestExceptionFactory,
            IMethodBuilder methodBuilder,
            IRequestBuilder requestBuilder,
            IHttpResponsePopulater httpResponsePopulater,
            IGuidProvider guidProvider,
            Type? controllerType = null,
            ILogger<T>? logger = null)
        {
            _host = host;
            _resilienceHttpClientProvider = resilienceHttpClientProvider;
            _clientInvocationProvider = clientInvocationProvider;
            _clientRequestExceptionFactory = clientRequestExceptionFactory;
            _methodBuilder = methodBuilder;
            _requestBuilder = requestBuilder;
            _httpResponsePopulater = httpResponsePopulater;
            _guidProvider = guidProvider;
            _controllerType = controllerType;
            _logger = logger;
        }

        protected override async Task InterceptAsync(
            IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> _)
        {
            await ProcessInvocationAsync(invocation, resultType: null)
                .ConfigureAwait(false);
        }

        protected override async Task<TResult> InterceptAsync<TResult>(
            IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> _)
        {
            return (TResult)await ProcessInvocationAsync(invocation, resultType: typeof(TResult))
                .ConfigureAwait(false);
        }

        private async Task<object> ProcessInvocationAsync(IInvocation invocation, Type? resultType)
        {
            var requestId = _guidProvider.Create();
            using var loggingScope = _logger?.BeginScope("Processing request {requestId}.", requestId);

            HttpResponse? httpResponse = null;
            try
            {
                var clientInvocation = _clientInvocationProvider.Get(interfaceType: typeof(T), _controllerType, invocation);
                var clientMethod = _methodBuilder.Build(clientInvocation.ClientType, clientInvocation.MethodInfo);

                var request = _requestBuilder.Build(requestId, _host, clientMethod, clientInvocation.MethodArguments);

                var response = await _resilienceHttpClientProvider
                    .Create(clientInvocation.ResiliencePolicyProvider)
                    .ExecuteAsync(request)
                    .ConfigureAwait(false);

                var populatedResponse = _httpResponsePopulater.Populate(response, resultType);

                if (typeof(HttpResponse).IsAssignableFrom(resultType))
                {
                    _logger?.LogDebug("Processing request finished. Request id: '{requestId}'.", requestId);
                    return populatedResponse;
                }

                populatedResponse.EnsureSuccess();
                _logger?.LogDebug("Processing request finished. Request id: '{requestId}'.", requestId);
                return populatedResponse.GetType().GetProperty("Value")?.GetValue(populatedResponse)!;
            }
            catch (ClientValidationException e)
            {
                _logger?.LogError(e, "Processing request error. Request id: '{requestId}'.", requestId);
                e.InterfaceType = typeof(T);
                e.MethodInfo = invocation.Method;
                throw;
            }
            catch (ClientHttpRequestException e)
            {
                _logger?.LogError(e, "Processing request error. Request id: '{requestId}'.", requestId);
                throw _clientRequestExceptionFactory.WrapClientHttpRequestException(interfaceType: typeof(T), invocation.Method, e);
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Unexpected processing request error. Request id: '{requestId}'.", requestId);
                if (httpResponse is not null)
                    throw _clientRequestExceptionFactory.WrapException(interfaceType: typeof(T), invocation.Method, httpResponse, e);
                throw _clientRequestExceptionFactory.WrapException(interfaceType: typeof(T), invocation.Method, e);
            }
        }
    }
}
