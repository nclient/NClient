using System;
using Castle.DynamicProxy;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.ClientGeneration;
using NClient.Common.Helpers;
using NClient.Core.Interceptors;
using NClient.Core.Mappers;
using NClient.Core.Resilience;
using NClient.Core.Validation;
using NClient.Customizers;

namespace NClient
{
    /// <summary>
    /// The builder used to create the client with custom providers.
    /// </summary>
    public class NClientStandaloneBuilder<TRequest, TResponse> : INClientBuilder<TRequest, TResponse>
    {
        private static readonly IProxyGenerator ProxyGenerator = new ProxyGenerator();

        private readonly IHttpClientProvider<TRequest, TResponse> _httpClientProvider;
        private readonly IHttpMessageBuilderProvider<TRequest, TResponse> _httpMessageBuilderProvider;
        private readonly IHttpClientExceptionFactory<TRequest, TResponse> _httpClientExceptionFactory;
        private readonly IMethodResiliencePolicyProvider<TResponse> _methodResiliencePolicyProvider;
        private readonly ISerializerProvider _serializerProvider;
        private readonly IClientValidator _clientValidator;
        private readonly IClientInterceptorFactory _interfaceClientInterceptorFactory;
        private readonly IClientGenerator _clientGenerator;

        /// <summary>
        /// Creates the builder with custom providers.
        /// </summary>
        /// <param name="httpClientProvider">The provider that can create instances of <see cref="IHttpClient"/>.</param>
        /// <param name="httpMessageBuilderProvider">The provider that can create instances of <see cref="IHttpMessageBuilder"/>.</param>
        /// <param name="httpClientExceptionFactory">The factory that can create instances of <see cref="HttpClientException"/>.</param>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        public NClientStandaloneBuilder(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory,
            ISerializerProvider serializerProvider) 
            : this(
                httpClientProvider,
                httpMessageBuilderProvider,
                httpClientExceptionFactory,
                new DefaultMethodResiliencePolicyProvider<TResponse>(
                    new DefaultResiliencePolicyProvider<TResponse>()),
                serializerProvider)
        {
        }
        
        internal NClientStandaloneBuilder(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory,
            IMethodResiliencePolicyProvider<TResponse> methodResiliencePolicyProvider,
            ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));
            Ensure.IsNotNull(httpMessageBuilderProvider, nameof(httpMessageBuilderProvider));
            Ensure.IsNotNull(httpClientExceptionFactory, nameof(httpClientExceptionFactory));
            Ensure.IsNotNull(methodResiliencePolicyProvider, nameof(methodResiliencePolicyProvider));
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));

            _httpClientProvider = httpClientProvider;
            _httpMessageBuilderProvider = httpMessageBuilderProvider;
            _httpClientExceptionFactory = httpClientExceptionFactory;
            _methodResiliencePolicyProvider = methodResiliencePolicyProvider;
            _serializerProvider = serializerProvider;
            _clientValidator = new ClientValidator(ProxyGenerator);
            _clientGenerator = new ClientGenerator(ProxyGenerator);
            _interfaceClientInterceptorFactory = new ClientInterceptorFactory(ProxyGenerator, new AttributeMapper());
        }

        public INClientBuilderCustomizer<TInterface, TRequest, TResponse> Use<TInterface>(string host)
            where TInterface : class
        {
            Ensure.IsNotNull(host, nameof(host));
            _clientValidator
                .EnsureAsync<TInterface>(_interfaceClientInterceptorFactory)
                .GetAwaiter()
                .GetResult();

            return new BuilderCustomizer<TInterface, TRequest, TResponse>(
                host: new Uri(host),
                _clientGenerator,
                _interfaceClientInterceptorFactory,
                _httpClientProvider,
                _httpMessageBuilderProvider,
                _httpClientExceptionFactory,
                _methodResiliencePolicyProvider,
                _serializerProvider);
        }
    }
}
