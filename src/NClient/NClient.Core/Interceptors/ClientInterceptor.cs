using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Clients;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Core.Exceptions;
using NClient.Core.Exceptions.Factories;
using NClient.Core.RequestBuilders;

namespace NClient.Core.Interceptors
{
    internal class ClientInterceptor<T> : AsyncInterceptorBase
    {
        private readonly IProxyGenerator _proxyGenerator;
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly IRequestBuilder _requestBuilder;
        private readonly IResiliencePolicyProvider _defaultResiliencePolicyProvider;
        private readonly Type? _controllerType;
        private readonly ILogger<T>? _logger;

        public ClientInterceptor(
            IProxyGenerator proxyGenerator,
            IHttpClientProvider httpClientProvider,
            IRequestBuilder requestBuilder,
            IResiliencePolicyProvider defaultResiliencePolicyProvider,
            Type? controllerType = null,
            ILogger<T>? logger = null)
        {
            _proxyGenerator = proxyGenerator;
            _httpClientProvider = httpClientProvider;
            _requestBuilder = requestBuilder;
            _defaultResiliencePolicyProvider = defaultResiliencePolicyProvider;
            _controllerType = controllerType;
            _logger = logger;
        }

        protected override async Task InterceptAsync(
            IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> _)
        {
            var requestId = Guid.NewGuid();
            await InvokeWithLoggingExceptionsAsync(ProcessInvocationAsync<HttpResponse>, invocation, requestId).ConfigureAwait(false);
        }

        protected override async Task<TResult> InterceptAsync<TResult>(
            IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> _)
        {
            var requestId = Guid.NewGuid();
            return await InvokeWithLoggingExceptionsAsync(ProcessInvocationAsync<TResult>, invocation, requestId).ConfigureAwait(false);
        }

        private async Task<TResult> ProcessInvocationAsync<TResult>(IInvocation invocation, Guid requestId)
        {
            using var loggingScope = _logger?.BeginScope("Processing request {requestId}.", requestId);

            var clientType = _controllerType ?? typeof(T);
            var clientMethod = _controllerType is null
                ? invocation.Method
                : TryGetMethodImpl(_controllerType, interfaceType: typeof(T), invocation.Method);
            var clientMethodArguments = invocation.Arguments;
            var resiliencePolicyProvider = _defaultResiliencePolicyProvider;

            if (IsDefaultNClientMethod(invocation.Method))
            {
                var clientMethodInvocation = invocation.Arguments[0];
                if (invocation.Arguments[0] is null)
                    throw InnerExceptionFactory.NullArgument(invocation.Method.GetParameters()[0].Name);
                var customResiliencePolicyProvider = (IResiliencePolicyProvider?)invocation.Arguments[1];

                var keepDataInterceptor = new KeepDataInterceptor();
                var proxyClient = _proxyGenerator.CreateInterfaceProxyWithoutTarget(typeof(T), keepDataInterceptor);
                ((LambdaExpression)clientMethodInvocation).Compile().DynamicInvoke(proxyClient);
                var innerInvocation = keepDataInterceptor.Invocation!;

                clientType = _controllerType ?? typeof(T);
                clientMethod = _controllerType is null
                    ? innerInvocation.Method
                    : TryGetMethodImpl(_controllerType, interfaceType: typeof(T), innerInvocation.Method);
                clientMethodArguments = innerInvocation.Arguments;
                resiliencePolicyProvider = customResiliencePolicyProvider ?? _defaultResiliencePolicyProvider;
            }

            var result = await ExecuteRequestAsync<TResult>(clientType!, clientMethod!, clientMethodArguments, resiliencePolicyProvider, requestId)
                .ConfigureAwait(false);

            _logger?.LogDebug("Processing request finished. Request id: '{requestId}'.", requestId);
            return result;
        }

        private async Task<TResult> ExecuteRequestAsync<TResult>(
            Type clientType, MethodInfo clientMethod, object[] clientMethodArguments, IResiliencePolicyProvider resiliencePolicyProvider, Guid requestId)
        {
            var request = _requestBuilder.Build(clientType, clientMethod, clientMethodArguments);
            _logger?.LogDebug("Start sending {requestMethod} request to '{requestUri}'. Request id: '{requestId}'.", request.Method, request.Uri, requestId);

            var responseBodyType = typeof(HttpResponse).IsAssignableFrom(typeof(TResult)) && typeof(TResult).IsGenericType
                ? typeof(TResult).GetGenericArguments().First()
                : typeof(HttpResponse) == typeof(TResult)
                    ? null
                    : typeof(TResult);

            var response = await resiliencePolicyProvider
                .Create()
                .ExecuteAsync(() => ExecuteRequestWithLoggingAsync(request, requestId, responseBodyType), requestId.ToString())
                .ConfigureAwait(false);

            if (typeof(HttpResponse).IsAssignableFrom(typeof(TResult)))
            {
                _logger?.LogDebug("Response with code {responseStatusCode} received. Request id: '{requestId}'.", response.StatusCode, requestId);
                return (TResult)(object)response;
            }

            if (!response.IsSuccessful)
                throw OuterExceptionFactory.HttpRequestFailed(response.StatusCode, response.ErrorMessage);

            _logger?.LogDebug("Response with code {responseStatusCode} received. Request id: '{requestId}'.", response.StatusCode, requestId);
            return (TResult)response.GetType().GetProperty("Value")!.GetValue(response);
        }

        private async Task<HttpResponse> ExecuteRequestWithLoggingAsync(HttpRequest request, Guid requestId, Type? bodyType = null)
        {
            var client = _httpClientProvider.Create();
            try
            {
                _logger?.LogDebug("Start sending request attempt. Request id: '{requestId}'.", requestId);
                var response = await client.ExecuteAsync(request, bodyType).ConfigureAwait(false);
                if (response.IsSuccessful)
                    _logger?.LogDebug("Request attempt finished with code {responseStatusCode} received. Request id: '{requestId}'.", response.StatusCode, requestId);
                else
                    _logger?.LogWarning(response.ErrorException, "Request attempt failed with code {responseStatusCode} and error message: '{errorMessage}'. Request id: '{requestId}'.",
                        response.StatusCode, response.ErrorMessage, requestId);
                return response;
            }
            catch (Exception e)
            {
                _logger?.LogWarning(e, "Request attempt failed with exception. Request id: '{requestId}'.", requestId);
                throw;
            }
        }

        private async Task<TResult> InvokeWithLoggingExceptionsAsync<TResult>(
            Func<IInvocation, Guid, Task<TResult>> processInvocation, IInvocation invocation, Guid requestId)
        {
            try
            {
                return await processInvocation(invocation, requestId).ConfigureAwait(false);
            }
            catch (NClientException e)
            {
                _logger?.LogError(e, "Processing request error. Request id: '{requestId}'.", requestId);
                throw;
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Unexpected processing request error. Request id: '{requestId}'.", requestId);
                throw;
            }
        }

        private static MethodInfo? TryGetMethodImpl(Type implType, Type interfaceType, MethodInfo interfaceMethod)
        {
            if (implType.GetInterfaces().All(x => x != interfaceType))
                return null;

            var interfaceMapping = implType.GetInterfaceMap(interfaceType);
            var methodPairs = interfaceMapping.InterfaceMethods
                .Zip(interfaceMapping.TargetMethods, (x, y) => (First: x, Second: y));
            return methodPairs.SingleOrDefault(x => x.First == interfaceMethod).Second;
        }

        //TODO: Solution is not good enough
        private static bool IsDefaultNClientMethod(MethodInfo method)
        {
            if (typeof(IResilienceNClient<>).GetMethods().Any(x => x.Name == method.Name))
                return true;
            if (typeof(IHttpNClient<>).GetMethods().Any(x => x.Name == method.Name))
                return true;

            return false;
        }
    }
}
