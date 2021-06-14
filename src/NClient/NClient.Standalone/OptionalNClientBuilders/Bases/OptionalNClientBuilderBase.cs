using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;

namespace NClient.OptionalNClientBuilders.Bases
{
    internal abstract class OptionalNClientBuilderBase<TBuilder, TResult>
        : IOptionalBuilderBase<TBuilder, TResult>
        where TBuilder : class, IOptionalBuilderBase<TBuilder, TResult>
    {
        protected IHttpClientProvider HttpClientProvider;
        protected ISerializerProvider SerializerProvider;
        protected IResiliencePolicyProvider? ResiliencePolicyProvider;
        protected ILoggerFactory? LoggerFactory;
        protected ILogger<TResult>? Logger;

        protected OptionalNClientBuilderBase(
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider)
        {
            HttpClientProvider = httpClientProvider;
            SerializerProvider = serializerProvider;
        }

        public TBuilder WithCustomHttpClient(IHttpClientProvider httpClientProvider)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));

            HttpClientProvider = httpClientProvider;
            return (this as TBuilder)!;
        }

        public TBuilder WithCustomSerializer(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));

            SerializerProvider = serializerProvider;
            return (this as TBuilder)!;
        }

        public TBuilder WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider)
        {
            Ensure.IsNotNull(resiliencePolicyProvider, nameof(resiliencePolicyProvider));

            ResiliencePolicyProvider = resiliencePolicyProvider;
            return (this as TBuilder)!;
        }

        public TBuilder WithLogging(ILoggerFactory loggerFactory)
        {
            Ensure.IsNotNull(loggerFactory, nameof(loggerFactory));

            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger<TResult>();
            return (this as TBuilder)!;
        }

        public TBuilder WithLogging(ILogger<TResult> logger)
        {
            Ensure.IsNotNull(logger, nameof(logger));

            Logger = logger;
            return (this as TBuilder)!;
        }

        public abstract TResult Build();
    }
}