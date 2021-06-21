using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using NClient.Core.Handling;
using NClient.Core.HttpClients;
using NClient.Core.Interceptors;
using NClient.Core.Resilience;
using NClient.Core.Serialization;

namespace NClient.Core.Validation
{
    internal interface IClientValidator
    {
        void Ensure<TInterface>(IClientInterceptorFactory clientInterceptorFactory)
            where TInterface : class;

        void Ensure<TInterface, TController>(IClientInterceptorFactory clientInterceptorFactory)
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

        public void Ensure<TInterface>(IClientInterceptorFactory clientInterceptorFactory)
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

            EnsureValidity(client);
        }

        public void Ensure<TInterface, TController>(IClientInterceptorFactory clientInterceptorFactory)
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

            EnsureValidity(client);
        }

        private static void EnsureValidity<T>(T client) where T : class
        {
            foreach (var methodInfo in typeof(T).GetMethods())
            {
                var parameters = methodInfo.GetParameters().Select(GetDefaultParameter).ToArray();
                methodInfo.Invoke(client, parameters);
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
