using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Mapping;
using NClient.Exceptions;
using NClient.Resilience;
using NClient.Standalone.Ensuring;
using NClient.Standalone.Handling;
using NClient.Standalone.HttpClients;
using NClient.Standalone.Interceptors;
using NClient.Standalone.Interceptors.Validation;
using NClient.Standalone.Mapping;
using NClient.Standalone.Resilience;
using NClient.Standalone.Serialization;

namespace NClient.Standalone.Validation
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
                .Create<TClient, IHttpRequest, object>(
                    FakeHost,
                    new StubHttpClientProvider(),
                    new StubHttpMessageBuilderProvider(),
                    new StubSerializerProvider(),
                    new ResponseValidator<IHttpRequest, object>(new StubEnsuringSettings<IHttpRequest, object>()),
                    new[] { new StubClientHandler<IHttpRequest, object>() },
                    new[] { new StubResponseMapper() },
                    new MethodResiliencePolicyProviderAdapter<IHttpRequest, object>(
                        new StubResiliencePolicyProvider<IHttpRequest, object>()));
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
