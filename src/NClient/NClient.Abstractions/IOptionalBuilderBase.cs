using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;

namespace NClient.Abstractions
{
    public interface IOptionalBuilderBase<TBuilder, TResult>
        where TBuilder : IOptionalBuilderBase<TBuilder, TResult>
    {
        /// <summary>
        /// Sets custom <see cref="IHttpClientProvider"/> used to create instances of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="httpClientProvider">The provider that can create instances of <see cref="IHttpClient"/> instances.</param>
        TBuilder WithCustomHttpClient(IHttpClientProvider httpClientProvider);

        /// <summary>
        /// Sets custom <see cref="ISerializerProvider"/> used to create instances of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/> instances.</param>
        TBuilder WithCustomSerializer(ISerializerProvider serializerProvider);

        /// <summary>
        /// Sets collection of <see cref="IClientHandler"/> used to handle HTTP requests and responses />.
        /// </summary>
        /// <param name="handlers">The collection of handlers.</param>
        /// <param name="useDefaults">The flag for using default handlers. If you use false, the exception will not be thrown in case of failed HTTP status code.</param>
        TBuilder WithCustomHandlers(IReadOnlyCollection<IClientHandler> handlers);

        /// <summary>
        /// Sets custom <see cref="IResiliencePolicyProvider"/> used to create instances of <see cref="IResiliencePolicy"/>.
        /// </summary>
        /// <param name="resiliencePolicyProvider">The provider that can create instances of <see cref="IResiliencePolicy"/> instances.</param>
        TBuilder WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider);

        /// <summary>
        /// Sets custom <see cref="ILoggerFactory"/> used to create instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="loggerFactory">The factory that can create instances of <see cref="ILogger"/>.</param>
        TBuilder WithLogging(ILoggerFactory loggerFactory);

        /// <summary>
        /// Creates <see cref="TResult"/>.
        /// </summary>
        TResult Build();
    }
}