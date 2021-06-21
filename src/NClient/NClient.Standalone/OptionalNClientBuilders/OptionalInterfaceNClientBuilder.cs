using System;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.ClientGeneration;
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
        private readonly IClientGenerator _clientGenerator;
        private readonly IClientInterceptorFactory _clientInterceptorFactory;

        public OptionalInterfaceNClientBuilder(
            Uri host,
            IClientGenerator clientGenerator,
            IClientInterceptorFactory clientInterceptorFactory,
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider)
            : base(httpClientProvider, serializerProvider)
        {
            _host = host;
            _clientGenerator = clientGenerator;
            _clientInterceptorFactory = clientInterceptorFactory;
        }

        public override TInterface Build()
        {
            var interceptor = _clientInterceptorFactory.Create(
                _host,
                HttpClientProvider,
                SerializerProvider,
                ClientHandlers,
                GetOrCreateMethodResiliencePolicyProvider(),
                Logger);

            return _clientGenerator.CreateClient<TInterface>(interceptor);
        }

        public IOptionalNClientBuilder<TInterface> WithResiliencePolicy(
            Func<TInterface, Delegate> methodSelector, IResiliencePolicyProvider resiliencePolicyProvider)
        {
            AddSpecificResiliencePolicyProvider(methodSelector, resiliencePolicyProvider);
            return this;
        }
    }
}
