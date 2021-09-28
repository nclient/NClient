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
        private readonly ISerializerProvider _serializerProvider;
        private readonly IMethodResiliencePolicyProvider<TResponse>? _methodResiliencePolicyProvider;
        private readonly ILoggerFactory? _loggerFactory;

        /// <summary>
        /// Creates the client factory with custom providers.
        /// </summary>
        /// <param name="httpClientProvider">The provider that can create instances of <see cref="IHttpClient"/> instances.</param>
        /// <param name="httpMessageBuilderProvider">The provider that can create instances of <see cref="httpMessageBuilder"/> instances.</param>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/> instances.</param>
        /// <param name="resiliencePolicyProvider">The provider that can create instances of <see cref="IResiliencePolicy"/> for specific method.</param>
        /// <param name="loggerFactory">The factory that can create instances of <see cref="ILogger"/>.</param>
        public NClientStandaloneFactory(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            ISerializerProvider serializerProvider,
            IResiliencePolicyProvider<TResponse>? resiliencePolicyProvider = null,
            ILoggerFactory? loggerFactory = null)
            : this(httpClientProvider, httpMessageBuilderProvider, serializerProvider, methodResiliencePolicyProvider: GetOrDefault(resiliencePolicyProvider), loggerFactory)
        {
        }

        /// <summary>
        /// Creates the client factory with custom providers.
        /// </summary>
        /// <param name="httpClientProvider">The provider that can create instances of <see cref="IHttpClient"/> instances.</param>
        /// <param name="httpMessageBuilderProvider">The provider that can create instances of <see cref="httpMessageBuilder"/> instances.</param>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/> instances.</param>
        /// <param name="methodResiliencePolicyProvider">The provider that can create instances of <see cref="IResiliencePolicy"/> for specific method.</param>
        /// <param name="loggerFactory">The factory that can create instances of <see cref="ILogger"/>.</param>
        public NClientStandaloneFactory(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            ISerializerProvider serializerProvider,
            IMethodResiliencePolicyProvider<TResponse>? methodResiliencePolicyProvider = null,
            ILoggerFactory? loggerFactory = null)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));
            Ensure.IsNotNull(httpMessageBuilderProvider, nameof(httpMessageBuilderProvider));
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));

            _httpClientProvider = httpClientProvider;
            _httpMessageBuilderProvider = httpMessageBuilderProvider;
            _serializerProvider = serializerProvider;
            _methodResiliencePolicyProvider = methodResiliencePolicyProvider;
            _loggerFactory = loggerFactory;
        }

        public TInterface Create<TInterface>(string host) where TInterface : class
        {
            Ensure.IsNotNull(host, nameof(host));

            return new NClientStandaloneBuilder<TRequest, TResponse>(_httpClientProvider, _httpMessageBuilderProvider, _serializerProvider)
                .Use<TInterface>(host)
                .TrySetResiliencePolicy(_methodResiliencePolicyProvider)
                .TrySetLogging(_loggerFactory)
                .Build();
        }

        private static DefaultMethodResiliencePolicyProvider<TResponse>? GetOrDefault(IResiliencePolicyProvider<TResponse>? resiliencePolicyProvider)
        {
            return resiliencePolicyProvider is not null
                ? new DefaultMethodResiliencePolicyProvider<TResponse>(resiliencePolicyProvider)
                : null;
        }
    }
}
