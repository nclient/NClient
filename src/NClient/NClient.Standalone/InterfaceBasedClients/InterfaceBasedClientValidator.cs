using System;
using Castle.DynamicProxy;
using NClient.Core.Helpers;
using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Helpers.ObjectToKeyValueConverters;
using NClient.Core.HttpClients;
using NClient.Core.Interceptors;
using NClient.Core.Interceptors.ClientInvocations;
using NClient.Core.Mappers;
using NClient.Core.MethodBuilders;
using NClient.Core.MethodBuilders.Providers;
using NClient.Core.RequestBuilders;
using NClient.Core.Resilience;
using NClient.Core.Validation;

namespace NClient.InterfaceBasedClients
{
    internal class InterfaceBasedClientValidator
    {
        public void Ensure<T>(IProxyGenerator proxyGenerator) where T : class
        {
            var clientInvocationProvider = new ClientInvocationProvider(proxyGenerator);
            var objectMemberManager = new ObjectMemberManager();
            var attributeMapper = new AttributeMapper();
            var guidProvider = new GuidProvider();

            var methodAttributeProvider = new MethodAttributeProvider(attributeMapper);
            var pathAttributeProvider = new PathAttributeProvider(attributeMapper);
            var headerAttributeProvider = new HeaderAttributeProvider();
            var paramAttributeProvider = new ParamAttributeProvider(attributeMapper);

            var clientMethodParamBuilder = new MethodParamBuilder(paramAttributeProvider);
            var clientMethodBuilder = new MethodBuilder(
                methodAttributeProvider,
                pathAttributeProvider,
                headerAttributeProvider,
                clientMethodParamBuilder);

            var requestBuilder = new RequestBuilder(
                host: new Uri("http://localhost:5000"),
                new RouteTemplateProvider(),
                new RouteProvider(objectMemberManager),
                new HttpMethodProvider(),
                new ObjectToKeyValueConverter(objectMemberManager));

            var resilienceHttpClientProvider = new ResilienceHttpClientProvider(
                new StubHttpClientProvider(),
                new StubResiliencePolicyProvider());

            var interceptor = new ClientInterceptor<T>(
                resilienceHttpClientProvider,
                clientInvocationProvider,
                clientMethodBuilder,
                requestBuilder,
                guidProvider);

            proxyGenerator
                .CreateInterfaceProxyWithoutTarget<T>(interceptor.ToInterceptor())
                .EnsureValidity();
        }
    }
}
