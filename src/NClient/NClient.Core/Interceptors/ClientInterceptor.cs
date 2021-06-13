using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Exceptions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Core.Exceptions;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers;
using NClient.Core.HttpClients;
using NClient.Core.Interceptors.ClientInvocations;
using NClient.Core.MethodBuilders;
using NClient.Core.MethodBuilders.Models;
using NClient.Core.RequestBuilders;
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
            _guidProvider = guidProvider;
            _controllerType = controllerType;
            _logger = logger;
        }

        protected override async Task InterceptAsync(
            IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> _)
        {
            var requestId = _guidProvider.Create();
            var clientInvocation = _clientInvocationProvider.Get(interfaceType: typeof(T), _controllerType, invocation);
            var clientMethod = _methodBuilder.Build(clientInvocation.ClientType, clientInvocation.MethodInfo);
            await InvokeWithLoggingExceptionsAsync(async (reqId, inv, method) =>
            {
                return (await ProcessInvocationAsync<HttpResponse>(reqId, inv, method).ConfigureAwait(false))
                    .EnsureSuccess();
            }, requestId, clientInvocation, clientMethod).ConfigureAwait(false);
        }

        protected override async Task<TResult> InterceptAsync<TResult>(
            IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> _)
        {
            var requestId = _guidProvider.Create();
            var clientInvocation = _clientInvocationProvider.Get(interfaceType: typeof(T), _controllerType, invocation);
            var clientMethod = _methodBuilder.Build(clientInvocation.ClientType, clientInvocation.MethodInfo);
            return await InvokeWithLoggingExceptionsAsync(ProcessInvocationAsync<TResult>, requestId, clientInvocation, clientMethod).ConfigureAwait(false);
        }

        private async Task<TResult> ProcessInvocationAsync<TResult>(Guid requestId, ClientInvocation clientInvocation, Method method)
        {
            using var loggingScope = _logger?.BeginScope("Processing request {requestId}.", requestId);

            var request = _requestBuilder.Build(requestId, _host, method, clientInvocation.MethodArguments);
            var result = await ExecuteRequestAsync<TResult>(request, clientInvocation.ResiliencePolicyProvider)
                .ConfigureAwait(false);

            _logger?.LogDebug("Processing request finished. Request id: '{requestId}'.", requestId);
            return result;
        }

        private async Task<TResult> ExecuteRequestAsync<TResult>(HttpRequest request, IResiliencePolicyProvider? resiliencePolicyProvider)
        {
            var (bodyType, errorType) = GetBodyAndErrorType<TResult>();

            var response = await _resilienceHttpClientProvider
                .Create(resiliencePolicyProvider)
                .ExecuteAsync(request, bodyType, errorType)
                .ConfigureAwait(false);

            if (typeof(HttpResponse).IsAssignableFrom(typeof(TResult)))
                return (TResult)(object)response;

            response.EnsureSuccess();
            return (TResult)response.GetType().GetProperty("Value")!.GetValue(response);
        }

        private static (Type? BodyType, Type? ErrorType) GetBodyAndErrorType<TResult>()
        {
            var resultType = typeof(TResult);

            if (resultType == typeof(HttpResponse))
                return (null, null);

            if (IsAssignableFromGeneric<TResult>(typeof(HttpResponseWithError<>)))
                return (null, resultType.GetGenericArguments().Single());

            if (IsAssignableFromGeneric<TResult>(typeof(HttpResponse<>)))
                return (resultType.GetGenericArguments().Single(), null);

            if (IsAssignableFromGeneric<TResult>(typeof(HttpResponseWithError<,>)))
                return (resultType.GetGenericArguments()[0], resultType.GetGenericArguments()[1]);

            return (resultType, null);
        }

        private static bool IsAssignableFromGeneric<TSource>(Type destType)
        {
            return typeof(TSource).IsGenericType && typeof(TSource).GetGenericTypeDefinition().IsAssignableFrom(destType.GetGenericTypeDefinition());
        }

        private async Task<TResult> InvokeWithLoggingExceptionsAsync<TResult>(
            Func<Guid, ClientInvocation, Method, Task<TResult>> processInvocation, Guid requestId, ClientInvocation clientInvocation, Method method)
        {
            try
            {
                return await processInvocation(requestId, clientInvocation, method).ConfigureAwait(false);
            }
            catch (ClientValidationException e)
            {
                _logger?.LogError(e, "Processing request error. Request id: '{requestId}'.", requestId);
                e.Method = method;
                throw;
            }
            catch (ClientHttpRequestException e)
            {
                _logger?.LogError(e, "Processing request error. Request id: '{requestId}'.", requestId);
                throw _clientRequestExceptionFactory.WrapClientHttpRequestException(clientInvocation.ClientType, method, e);
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Unexpected processing request error. Request id: '{requestId}'.", requestId);
                throw _clientRequestExceptionFactory.WrapException(clientInvocation.ClientType, method, e);
            }
        }
    }
}
