using System;
using Castle.DynamicProxy;
using NClient.Core;
using NClient.Core.Helpers;
using NClient.Core.Interceptors;
using NClient.Core.RequestBuilders;
using NClient.Core.Validators;
using NClient.InterfaceProxy.Attributes;
using NClient.Providers.HttpClient;
using NClient.Providers.Resilience;

namespace NClient.InterfaceProxy.Validators
{
    internal class ClientInterfaceValidator
    {
        public void Ensure<T>(IProxyGenerator proxyGenerator) where T : class, INClient
        {
            var attributeHelper = new AttributeHelper();
            var requestBuilder = new RequestBuilder(
                host: new Uri("http://localhost:5000"),
                new RouteBuilder(attributeHelper),
                new HttpMethodProvider(attributeHelper),
                new ParameterProvider(attributeHelper),
                new ObjectToKeyValueConverter(),
                attributeHelper);
            var interceptor = new ClientInterceptor<T>(proxyGenerator, new StubHttpClientProvider(), requestBuilder, new StubResiliencePolicyProvider());
            
            proxyGenerator
                .CreateInterfaceProxyWithoutTarget<T>(interceptor.ToInterceptor())
                .EnsureValidity();
        }
    }
}
