﻿using System;
using Castle.DynamicProxy;
using NClient.Core;
using NClient.Core.Helpers;
using NClient.Core.Interceptors;
using NClient.Core.Mappers;
using NClient.Core.RequestBuilders;
using NClient.Core.Validators;
using NClient.Providers.HttpClient;
using NClient.Providers.Resilience;

namespace NClient.InterfaceProxy.Validators
{
    internal class ClientInterfaceValidator
    {
        public void Ensure<T>(IProxyGenerator proxyGenerator) where T : class, INClient
        {
            var attributeMapper = new AttributeMapper();

            var requestBuilder = new RequestBuilder(
                host: new Uri("http://localhost:5000"),
                new RouteTemplateProvider(attributeMapper),
                new RouteProvider(),
                new HttpMethodProvider(attributeMapper),
                new ParameterProvider(attributeMapper),
                new ObjectToKeyValueConverter());
            var interceptor = new ClientInterceptor<T>(proxyGenerator, new StubHttpClientProvider(), requestBuilder, new StubResiliencePolicyProvider());
            
            proxyGenerator
                .CreateInterfaceProxyWithoutTarget<T>(interceptor.ToInterceptor())
                .EnsureValidity();
        }
    }
}
