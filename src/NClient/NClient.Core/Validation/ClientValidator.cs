using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using NClient.Core.Handling;
using NClient.Core.HttpClients;
using NClient.Core.Interceptors;
using NClient.Core.Resilience;
using NClient.Core.Serialization;
using NClient.Exceptions;

namespace NClient.Core.Validation
{
    internal interface IClientValidator
    {
        Task EnsureAsync<TInterface>(IClientInterceptorFactory clientInterceptorFactory)
            where TInterface : class;

        Task EnsureAsync<TInterface, TController>(IClientInterceptorFactory clientInterceptorFactory)
            where TInterface : class
            where TController : TInterface;
    }

    internal class ClientValidator : IClientValidator
    {
        private static readonly Uri FakeHost = new("http://localhost:5000");

        private readonly IProxyGenerator _proxyGenerator;

        public ClientValidator(IProxyGenerator proxyGenerator)
        {
            _proxyGenerator = proxyGenerator;
        }

        public async Task EnsureAsync<TInterface>(IClientInterceptorFactory clientInterceptorFactory)
            where TInterface : class
        {
            var interceptor = clientInterceptorFactory
                .Create<TInterface>(
                    FakeHost,
                    new StubHttpClientProvider(),
                    new StubSerializerProvider(),
                    new[] { new StubClientHandler() },
                    new DefaultMethodResiliencePolicyProvider(new DefaultResiliencePolicyProvider()));
            var client = _proxyGenerator.CreateInterfaceProxyWithoutTarget<TInterface>(interceptor.ToInterceptor());

            await EnsureValidityAsync(client).ConfigureAwait(false);
        }

        public async Task EnsureAsync<TInterface, TController>(IClientInterceptorFactory clientInterceptorFactory)
            where TInterface : class
            where TController : TInterface
        {
            var interceptor = clientInterceptorFactory
                .Create<TInterface, TController>(
                    FakeHost,
                    new StubHttpClientProvider(),
                    new StubSerializerProvider(),
                    new[] { new StubClientHandler() },
                    new DefaultMethodResiliencePolicyProvider(new DefaultResiliencePolicyProvider()));
            var client = _proxyGenerator.CreateInterfaceProxyWithoutTarget<TInterface>(interceptor.ToInterceptor());

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
