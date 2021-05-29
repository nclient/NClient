using System;
using Castle.DynamicProxy;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Core.Interceptors;

namespace NClient.OptionalNClientBuilders
{
    internal class OptionalControllerClientBuilder<TInterface, TController> : OptionalNClientBuilderBase<TInterface>
        where TInterface : class
        where TController : TInterface
    {
        public OptionalControllerClientBuilder(
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
                .Create<TInterface, TController>(Host, HttpClientProvider, SerializerProvider, ResiliencePolicyProvider, Logger);
            return CreateClient(interceptor);
        }
    }
}
