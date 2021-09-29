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
    internal class BuilderCustomizer<TInterface, TRequest, TResponse> :
        CommonCustomizer<INClientBuilderCustomizer<TInterface, TRequest, TResponse>, TInterface, TRequest, TResponse>,
        INClientBuilderCustomizer<TInterface, TRequest, TResponse>
        where TInterface : class
    {
        private readonly Uri _host;
        private readonly IClientGenerator _clientGenerator;
        private readonly IClientInterceptorFactory _clientInterceptorFactory;

        public BuilderCustomizer(
            Uri host,
            IClientGenerator clientGenerator,
            IClientInterceptorFactory clientInterceptorFactory,
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory,
            IMethodResiliencePolicyProvider<TResponse> methodResiliencePolicyProvider,
            ISerializerProvider serializerProvider)
            : base(httpClientProvider, httpMessageBuilderProvider, httpClientExceptionFactory, methodResiliencePolicyProvider, serializerProvider)
        {
            _host = host;
            _clientGenerator = clientGenerator;
            _clientInterceptorFactory = clientInterceptorFactory;
        }

        public INClientBuilderCustomizer<TInterface, TRequest, TResponse> WithResiliencePolicy(
            Expression<Func<TInterface, Delegate>> methodSelector, IResiliencePolicyProvider<TResponse> resiliencePolicyProvider)
        {
            AddSpecificResiliencePolicyProvider(methodSelector, resiliencePolicyProvider);
            return this;
        }
        
        public INClientBuilderCustomizer<TInterface, TRequest, TResponse> WithLogging(ILogger<TInterface> logger)
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
                HttpMessageBuilderProvider,
                HttpClientExceptionFactory,
                SerializerProvider,
                ClientHandlers,
                CreateMethodResiliencePolicyProvider(),
                Logger);

            return _clientGenerator.CreateClient<TInterface>(interceptor);
        }
    }
}
