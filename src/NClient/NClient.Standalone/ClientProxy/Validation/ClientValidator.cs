using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using NClient.Exceptions;
using NClient.Providers.Results;
using NClient.Providers.Transport;
using NClient.Resilience;
using NClient.Standalone.Client.Handling;
using NClient.Standalone.Client.Resilience;
using NClient.Standalone.Client.Serialization;
using NClient.Standalone.Client.Transport;
using NClient.Standalone.Client.Validation;
using NClient.Standalone.ClientProxy.Interceptors;

namespace NClient.Standalone.ClientProxy.Validation
{
    internal interface IClientValidator
    {
        Task EnsureAsync<TClient>(IClientInterceptorFactory clientInterceptorFactory)
            where TClient : class;
    }

    internal class ClientValidator : IClientValidator
    {
        private static readonly Uri FakeHost = new("http://localhost:5000");

        private readonly IProxyGenerator _proxyGenerator;

        public ClientValidator(IProxyGenerator proxyGenerator)
        {
            _proxyGenerator = proxyGenerator;
        }

        public async Task EnsureAsync<TClient>(IClientInterceptorFactory clientInterceptorFactory)
            where TClient : class
        {
            var interceptor = clientInterceptorFactory
                .Create<TClient, IHttpRequest, IHttpResponse>(
                    FakeHost,
                    new StubSerializerProvider(),
                    new StubTransportProvider(),
                    new StubHttpMessageBuilderProvider(),
                    new[] { new StubClientHandlerProvider<IHttpRequest, IHttpResponse>() },
                    new MethodResiliencePolicyProviderAdapter<IHttpRequest, IHttpResponse>(
                        new StubResiliencePolicyProvider<IHttpRequest, IHttpResponse>()),
                    Array.Empty<IResultBuilderProvider<IHttpResponse>>(),
                    Array.Empty<IResultBuilderProvider<IHttpResponse>>(),
                    new[] { new StubResponseValidatorProvider<IHttpRequest, IHttpResponse>() });
            var client = _proxyGenerator.CreateInterfaceProxyWithoutTarget<TClient>(interceptor.ToInterceptor());

            await EnsureValidityAsync(client).ConfigureAwait(false);
        }
        
        private static async Task EnsureValidityAsync<T>(T client) where T : class
        {
            foreach (var methodInfo in typeof(T).GetMethods())
            {
                var parameters = methodInfo.GetParameters().Select(GetDefaultParameter).ToArray();

                try
                {
                    var result = methodInfo.Invoke(client, parameters);
                    if (result is Task task)
                        await task.ConfigureAwait(false);
                }
                catch (ClientValidationException)
                {
                    throw;
                }
                catch
                {
                    // ignored
                }
            }
        }

        private static object? GetDefaultParameter(ParameterInfo parameter)
        {
            return parameter.ParameterType.IsValueType
                ? Activator.CreateInstance(parameter.ParameterType)
                : null;
        }
    }
}
