using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Core.Resilience;
using NClient.Extensions;

namespace NClient
{
    /// <summary>
    /// The factory used to create the client with custom providers.
    /// </summary>
    public class NClientStandaloneFactory<TRequest, TResponse> : INClientFactory
    {
        private readonly IHttpClientProvider<TRequest, TResponse> _httpClientProvider;
        private readonly IHttpMessageBuilderProvider<TRequest, TResponse> _httpMessageBuilderProvider;
        private readonly IHttpClientExceptionFactory<TRequest, TResponse> _httpClientExceptionFactory;
        private readonly ISerializerProvider _serializerProvider;
        private readonly IMethodResiliencePolicyProvider<TRequest, TResponse>? _methodResiliencePolicyProvider;
        private readonly ILoggerFactory? _loggerFactory;

        /// <summary>
        /// Creates the client factory with custom providers.
        /// </summary>
        /// <param name="httpClientProvider">The provider that can create instances of <see cref="IHttpClient"/>.</param>
        /// <param name="httpMessageBuilderProvider">The provider that can create instances of <see cref="IHttpMessageBuilder"/>.</param>
        /// <param name="httpClientExceptionFactory">The factory that can create instances of <see cref="HttpClientException"/></param>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        /// <param name="resiliencePolicyProvider">The provider that can create instances of <see cref="IResiliencePolicy"/> for specific method.</param>
        /// <param name="loggerFactory">The factory that can create instances of <see cref="ILogger"/>.</param>
        public NClientStandaloneFactory(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory,
            ISerializerProvider serializerProvider,
            IResiliencePolicyProvider<TRequest, TResponse>? resiliencePolicyProvider = null,
            ILoggerFactory? loggerFactory = null)
            : this(httpClientProvider, httpMessageBuilderProvider, httpClientExceptionFactory, serializerProvider, methodResiliencePolicyProvider: GetOrDefault(resiliencePolicyProvider), loggerFactory)
        {
        }

        /// <summary>
        /// Creates the client factory with custom providers.
        /// </summary>
        /// <param name="httpClientProvider">The provider that can create instances of <see cref="IHttpClient"/>.</param>
        /// <param name="httpMessageBuilderProvider">The provider that can create instances of <see cref="IHttpMessageBuilder"/>.</param>
        /// <param name="httpClientExceptionFactory">The factory that can create instances of <see cref="HttpClientException"/></param>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        /// <param name="methodResiliencePolicyProvider">The provider that can create instances of <see cref="IResiliencePolicy"/> for specific method.</param>
        /// <param name="loggerFactory">The factory that can create instances of <see cref="ILogger"/>.</param>
        public NClientStandaloneFactory(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory,
            ISerializerProvider serializerProvider,
            IMethodResiliencePolicyProvider<TRequest, TResponse>? methodResiliencePolicyProvider = null,
            ILoggerFactory? loggerFactory = null)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));
            Ensure.IsNotNull(httpMessageBuilderProvider, nameof(httpMessageBuilderProvider));
            Ensure.IsNotNull(httpClientExceptionFactory, nameof(httpClientExceptionFactory));
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));

            _httpClientProvider = httpClientProvider;
            _httpMessageBuilderProvider = httpMessageBuilderProvider;
            _httpClientExceptionFactory = httpClientExceptionFactory;
            _serializerProvider = serializerProvider;
            _methodResiliencePolicyProvider = methodResiliencePolicyProvider;
            _loggerFactory = loggerFactory;
        }

        public TInterface Create<TInterface>(string host) where TInterface : class
        {
            Ensure.IsNotNull(host, nameof(host));

            return new NClientStandaloneBuilder<TRequest, TResponse>(
                    _httpClientProvider, 
                    _httpMessageBuilderProvider, 
                    _httpClientExceptionFactory, 
                    _serializerProvider)
                .Use<TInterface>(host)
                .TrySetResiliencePolicy(_methodResiliencePolicyProvider)
                .TrySetLogging(_loggerFactory)
                .Build();
        }

        private static DefaultMethodResiliencePolicyProvider<TRequest, TResponse>? GetOrDefault(IResiliencePolicyProvider<TRequest, TResponse>? resiliencePolicyProvider)
        {
            return resiliencePolicyProvider is not null
                ? new DefaultMethodResiliencePolicyProvider<TRequest, TResponse>(resiliencePolicyProvider)
                : null;
        }
    }
}
