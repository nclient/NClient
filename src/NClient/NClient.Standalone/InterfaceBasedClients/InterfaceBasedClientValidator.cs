using System;
using Castle.DynamicProxy;
using NClient.Core.Helpers.ObjectToKeyValueConverters;
using NClient.Core.HttpClients;
using NClient.Core.Interceptors;
using NClient.Core.Interceptors.ClientInvocations;
using NClient.Core.Mappers;
using NClient.Core.RequestBuilders;
using NClient.Core.Resilience;
using NClient.Core.Validation;

namespace NClient.InterfaceBasedClients
{
    internal class InterfaceBasedClientValidator
    {
        public void Ensure<T>(IProxyGenerator proxyGenerator) where T : class
        {
            var attributeMapper = new AttributeMapper();
            var clientInvocationProvider = new ClientInvocationProvider(proxyGenerator);

            var requestBuilder = new RequestBuilder(
                host: new Uri("http://localhost:5000"),
                new RouteTemplateProvider(attributeMapper),
                new RouteProvider(),
                new HttpMethodProvider(attributeMapper),
                new ParameterProvider(attributeMapper),
                new ObjectToKeyValueConverter());

            var resilienceHttpClientProvider = new ResilienceHttpClientProvider(
                new StubHttpClientProvider(),
                new StubResiliencePolicyProvider());

            var interceptor = new ClientInterceptor<T>(
                resilienceHttpClientProvider,
                clientInvocationProvider,
                requestBuilder);

            proxyGenerator
                .CreateInterfaceProxyWithoutTarget<T>(interceptor.ToInterceptor())
                .EnsureValidity();
        }
    }
}
