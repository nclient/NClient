using System;
using System.Linq.Expressions;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;

namespace NClient.Customizers
{
    internal class FactoryCustomizer<TRequest, TResponse> :
        CommonCustomizer<INClientFactoryCustomizer<TRequest, TResponse>, INClientFactory, TRequest, TResponse>,
        INClientFactoryCustomizer<TRequest, TResponse>
    {
        public FactoryCustomizer(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory,
            IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider,
            ISerializerProvider serializerProvider)
            : base(httpClientProvider, httpMessageBuilderProvider, httpClientExceptionFactory, methodResiliencePolicyProvider, serializerProvider)
        {
        }

        public override INClientFactory Build()
        {
            return new NClientStandaloneFactory<TRequest, TResponse>(
                HttpClientProvider,
                HttpMessageBuilderProvider,
                HttpClientExceptionFactory,
                SerializerProvider,
                CreateMethodResiliencePolicyProvider(),
                LoggerFactory);
        }

        public INClientFactoryCustomizer<TRequest, TResponse> WithResiliencePolicy<TInterface>(
            Expression<Func<TInterface, Delegate>> methodSelector, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider)
        {
            AddSpecificResiliencePolicyProvider(methodSelector, resiliencePolicyProvider);
            return this;
        }
    }
}
