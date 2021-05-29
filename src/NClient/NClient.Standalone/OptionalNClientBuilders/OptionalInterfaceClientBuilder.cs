using System;
using Castle.DynamicProxy;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Core.Interceptors;

namespace NClient.OptionalNClientBuilders
{
    internal class OptionalInterfaceClientBuilder<TInterface> : OptionalNClientBuilderBase<TInterface>
        where TInterface : class
    {
        public OptionalInterfaceClientBuilder(
            Uri host,
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider,
            IClientInterceptorFactory clientInterceptorFactory,
            IProxyGenerator proxyGenerator)
            : base(host, httpClientProvider, serializerProvider, clientInterceptorFactory, proxyGenerator)
        {
        }

        public override TInterface Build()
        {
            var interceptor = ClientInterceptorFactory
                .Create(Host, HttpClientProvider, SerializerProvider, ResiliencePolicyProvider, Logger);
            return CreateClient(interceptor);
        }
    }
}
