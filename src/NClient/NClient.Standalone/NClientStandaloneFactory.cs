using System;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Extensions;

namespace NClient
{
    public class NClientStandaloneFactory : INClientFactory
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly ISerializerProvider _serializerProvider;
        private readonly IResiliencePolicyProvider? _resiliencePolicyProvider;
        private readonly ILoggerFactory? _loggerFactory;

        public NClientStandaloneFactory(
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider,
            IResiliencePolicyProvider? resiliencePolicyProvider = null,
            ILoggerFactory? loggerFactory = null)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));

            _httpClientProvider = httpClientProvider;
            _serializerProvider = serializerProvider;
            _resiliencePolicyProvider = resiliencePolicyProvider;
            _loggerFactory = loggerFactory;
        }

        public TInterface Create<TInterface>(string host) where TInterface : class
        {
            Ensure.IsNotNull(host, nameof(host));

            return new NClientStandaloneBuilder(_httpClientProvider, _serializerProvider)
                .Use<TInterface>(host)
                .TrySetResiliencePolicy(_resiliencePolicyProvider)
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
                .TrySetResiliencePolicy(_resiliencePolicyProvider)
                .TrySetLogging(_loggerFactory)
                .Build();
        }
    }
}
