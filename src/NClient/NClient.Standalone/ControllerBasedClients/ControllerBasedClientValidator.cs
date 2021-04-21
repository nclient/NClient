using System;
using Castle.DynamicProxy;
using NClient.Core.Helpers;
using NClient.Core.HttpClients;
using NClient.Core.Interceptors;
using NClient.Core.Interceptors.ClientInvocations;
using NClient.Core.RequestBuilders;
using NClient.Core.Resilience;
using NClient.Core.Validators;
using NClient.Mappers;

namespace NClient.ControllerBasedClients
{
    internal class ControllerBasedClientValidator
    {
        public void Ensure<TInterface, TController>(IProxyGenerator proxyGenerator)
            where TInterface : class
            where TController : TInterface
        {
            var attributeMapper = new AspNetAttributeMapper();
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

            var interceptor = new ClientInterceptor<TInterface>(
                resilienceHttpClientProvider,
                clientInvocationProvider,
                requestBuilder,
                controllerType: typeof(TController));

            proxyGenerator
                .CreateInterfaceProxyWithoutTarget<TInterface>(interceptor.ToInterceptor())
                .EnsureValidity();
        }
    }
}
