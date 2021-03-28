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
            await InvokeWithLoggingExceptions(ProcessInvocationAsync<HttpRequest>, invocation).ConfigureAwait(false);
        }

        protected override async Task<TResult> InterceptAsync<TResult>(
            IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> _)
        {
            return await InvokeWithLoggingExceptions(ProcessInvocationAsync<TResult>, invocation).ConfigureAwait(false);
        }

        private async Task<TResult> ProcessInvocationAsync<TResult>(IInvocation invocation)
        {
            using var loggingScope = _logger?.BeginScope("Processing request {requestId}.", Guid.NewGuid());

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

            var result = await ExecuteRequestAsync<TResult>(clientType!, clientMethod!, clientMethodArguments, resiliencePolicyProvider)
                .ConfigureAwait(false);

            _logger?.LogInformation("Processing request finished.");
            return result;
        }

        private async Task<TResult> ExecuteRequestAsync<TResult>(
            Type clientType, MethodInfo clientMethod, object[] clientMethodArguments, IResiliencePolicyProvider resiliencePolicyProvider)
        {
            var request = _requestBuilder.Build(clientType, clientMethod, clientMethodArguments);
            _logger?.LogDebug($"Start sending {request.Method} request to '{request.Uri}'.");

            var client = _httpClientProvider.Create();

            var responseBodyType = typeof(HttpResponse).IsAssignableFrom(typeof(TResult)) && typeof(TResult).IsGenericType 
                ? typeof(TResult).GetGenericArguments().First()
                : typeof(TResult);
            var response = await resiliencePolicyProvider
                .Create()
                .ExecuteAsync(() => client.ExecuteAsync(request, responseBodyType))
                .ConfigureAwait(false);
            _logger?.LogDebug($"Response with code {response.StatusCode} received.");

            if (typeof(HttpResponse).IsAssignableFrom(typeof(TResult)))
                return (TResult)(object)response;

            if (!response.IsSuccessful)
            {
                _logger?.LogError($"Request finished with error code {response.StatusCode}: {response.ErrorMessage}");
                throw OuterExceptionFactory.HttpRequestFailed(response.StatusCode, response.ErrorMessage);
            }

            return (TResult)response.GetType().GetProperty("Value")!.GetValue(response);
        }

        private async Task<TResult> InvokeWithLoggingExceptions<TResult>(Func<IInvocation, Task<TResult>> processInvocation, IInvocation invocation)
        {
            try
            {
                return await processInvocation(invocation).ConfigureAwait(false);
            }
            catch (NClientException e)
            {
                _logger?.LogError($"Processing request error: {e.Message}");
                throw;
            }
            catch (Exception e)
            {
                _logger?.LogError($"Unexpected processing request error: {e.Message}");
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
