using System;
using System.Linq.Expressions;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;

namespace NClient.Customizers
{
    internal class FactoryCustomizer :
        CommonCustomizer<INClientFactoryCustomizer, INClientFactory>,
        INClientFactoryCustomizer
    {
        public FactoryCustomizer(
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider)
            : base(httpClientProvider, serializerProvider)
        {
        }

        public override INClientFactory Build()
        {
            return new NClientStandaloneFactory(
                HttpClientProvider,
                SerializerProvider,
                GetOrCreateMethodResiliencePolicyProvider(),
                LoggerFactory);
        }

        public INClientFactoryCustomizer WithResiliencePolicy<TInterface>(
            Expression<Func<TInterface, Delegate>> methodSelector, IResiliencePolicyProvider resiliencePolicyProvider)
        {
            AddSpecificResiliencePolicyProvider(methodSelector, resiliencePolicyProvider);
            return this;
        }
    }
}
