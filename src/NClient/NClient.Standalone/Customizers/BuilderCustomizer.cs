using System;
using System.Linq.Expressions;
using System.Threading;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.ClientGeneration;
using NClient.Common.Helpers;
using NClient.Core.Interceptors;

namespace NClient.Customizers
{
    internal class BuilderCustomizer<TInterface> :
        CommonCustomizer<INClientBuilderCustomizer<TInterface>, TInterface>,
        INClientBuilderCustomizer<TInterface>
        where TInterface : class
    {
        private readonly Uri _host;
        private readonly IClientGenerator _clientGenerator;
        private readonly IClientInterceptorFactory _clientInterceptorFactory;

        public BuilderCustomizer(
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

        public INClientBuilderCustomizer<TInterface> WithResiliencePolicy(
            Expression<Func<TInterface, Delegate>> methodSelector, IResiliencePolicyProvider resiliencePolicyProvider)
        {
            AddSpecificResiliencePolicyProvider(methodSelector, resiliencePolicyProvider);
            return this;
        }
        
        public INClientBuilderCustomizer<TInterface> WithLogging(ILogger<TInterface> logger)
        {
            Ensure.IsNotNull(logger, nameof(logger));

            Interlocked.Exchange(ref Logger, logger);
            return this;
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
    }
}
