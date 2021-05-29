using System;
using Castle.DynamicProxy;
using NClient.Abstractions;
using NClient.Abstractions.Clients;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Core.Interceptors;
using NClient.OptionalNClientBuilders.Bases;

namespace NClient.OptionalNClientBuilders
{
    internal class OptionalInterfaceNClientBuilder<TInterface> :
        OptionalNClientBuilderBase<IOptionalNClientBuilder<TInterface>, TInterface>,
        IOptionalNClientBuilder<TInterface>
        where TInterface : class
    {
        private readonly Uri _host;
        private readonly IProxyGenerator _proxyGenerator;
        private readonly IClientInterceptorFactory _clientInterceptorFactory;

        public OptionalInterfaceNClientBuilder(
            Uri host,
            IProxyGenerator proxyGenerator,
            IClientInterceptorFactory clientInterceptorFactory,
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider)
            : base(httpClientProvider, serializerProvider)
        {
            _host = host;
            _proxyGenerator = proxyGenerator;
            _clientInterceptorFactory = clientInterceptorFactory;
        }

        public override TInterface Build()
        {
            var interceptor = _clientInterceptorFactory
                .Create(_host, HttpClientProvider, SerializerProvider, ResiliencePolicyProvider, Logger);
            return CreateClient(interceptor);
        }

        private TInterface CreateClient(IAsyncInterceptor asyncInterceptor)
        {
            return (TInterface)_proxyGenerator.CreateInterfaceProxyWithoutTarget(
                interfaceToProxy: typeof(TInterface),
                additionalInterfacesToProxy: new[] { typeof(IResilienceNClient<TInterface>), typeof(IHttpNClient<TInterface>) },
                interceptors: asyncInterceptor.ToInterceptor());
        }
    }
}
