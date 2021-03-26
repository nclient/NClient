using System;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using NClient.Abstractions.Clients;
using NClient.AspNetProxy.Attributes;
using NClient.Core.Helpers;
using NClient.Core.HttpClients;
using NClient.Core.Interceptors;
using NClient.Core.RequestBuilders;
using NClient.Core.Resilience;
using NClient.Core.Validators;

namespace NClient.AspNetProxy.Validators
{
    internal class ClientControllerValidator
    {
        public void Ensure<TInterface, TController>(IProxyGenerator proxyGenerator) 
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            var attributeMapper = new AspNetAttributeMapper();

            var requestBuilder = new RequestBuilder(
                host: new Uri("http://localhost:5000"),
                new RouteTemplateProvider(attributeMapper),
                new RouteProvider(),
                new HttpMethodProvider(attributeMapper),
                new ParameterProvider(attributeMapper),
                new ObjectToKeyValueConverter());

            var interceptor = new ClientInterceptor<TInterface>(
                proxyGenerator, 
                new StubHttpClientProvider(), 
                requestBuilder, 
                new StubResiliencePolicyProvider(),
                controllerType: typeof(TController));
            
            proxyGenerator
                .CreateInterfaceProxyWithoutTarget<TInterface>(interceptor.ToInterceptor())
                .EnsureValidity();
        }
    }
}
