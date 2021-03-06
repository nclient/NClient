using System;
using System.Collections.Generic;
using System.Reflection;
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
    public class NClientStandaloneFactory : INClientFactory
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly ISerializerProvider _serializerProvider;
        private readonly IMethodResiliencePolicyProvider? _methodResiliencePolicyProvider;
        private readonly ILoggerFactory? _loggerFactory;

        /// <summary>
        /// Creates the client factory with custom providers.
        /// </summary>
        /// <param name="httpClientProvider">The provider that can create instances of <see cref="IHttpClient"/> instances.</param>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/> instances.</param>
        /// <param name="resiliencePolicyProvider">The provider that can create instances of <see cref="IResiliencePolicy"/> for specific method.</param>
        /// <param name="loggerFactory">The factory that can create instances of <see cref="ILogger"/>.</param>
        public NClientStandaloneFactory(
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider,
            IResiliencePolicyProvider? resiliencePolicyProvider = null,
            ILoggerFactory? loggerFactory = null)
            : this(httpClientProvider, serializerProvider, methodResiliencePolicyProvider: GetOrDefault(resiliencePolicyProvider), loggerFactory)
        {
        }

        /// <summary>
        /// Creates the client factory with custom providers.
        /// </summary>
        /// <param name="httpClientProvider">The provider that can create instances of <see cref="IHttpClient"/> instances.</param>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/> instances.</param>
        /// <param name="methodResiliencePolicyProvider">The provider that can create instances of <see cref="IResiliencePolicy"/> for specific method.</param>
        /// <param name="loggerFactory">The factory that can create instances of <see cref="ILogger"/>.</param>
        public NClientStandaloneFactory(
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider,
            IMethodResiliencePolicyProvider? methodResiliencePolicyProvider = null,
            ILoggerFactory? loggerFactory = null)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));

            _httpClientProvider = httpClientProvider;
            _serializerProvider = serializerProvider;
            _methodResiliencePolicyProvider = methodResiliencePolicyProvider;
            _loggerFactory = loggerFactory;
        }

        public TInterface Create<TInterface>(string host) where TInterface : class
        {
            Ensure.IsNotNull(host, nameof(host));

            return new NClientStandaloneBuilder(_httpClientProvider, _serializerProvider)
                .Use<TInterface>(host)
                .TrySetResiliencePolicy(_methodResiliencePolicyProvider)
                .TrySetLogging(_loggerFactory)
                .Build();
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Create<T> method.")]
        public TInterface Create<TInterface, TController>(string host)
            where TInterface : class
            where TController : TInterface
        {
            Ensure.IsNotNull(host, nameof(host));

            return new NClientStandaloneBuilder(_httpClientProvider, _serializerProvider)
                .Use<TInterface, TController>(host)
                .TrySetResiliencePolicy(_methodResiliencePolicyProvider)
                .TrySetLogging(_loggerFactory)
                .Build();
        }

        private static DefaultMethodResiliencePolicyProvider? GetOrDefault(IResiliencePolicyProvider? resiliencePolicyProvider)
        {
            return resiliencePolicyProvider is not null
                ? new DefaultMethodResiliencePolicyProvider(resiliencePolicyProvider)
                : null;
        }
    }
}
