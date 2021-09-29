using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;

namespace NClient.Abstractions
{
    public interface INClientCommonCustomizer<TCustomizer, TResult, TRequest, TResponse>
        where TCustomizer : INClientCommonCustomizer<TCustomizer, TResult, TRequest, TResponse>
    {
        /// <summary>
        /// Sets custom <see cref="IHttpClientProvider"/> used to create instances of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="httpClientProvider">The provider that can create instances of <see cref="IHttpClient"/>.</param>
        /// <param name="httpMessageBuilderProvider">The provider that can create instances of <see cref="IHttpMessageBuilder"/>.</param>
        /// <param name="httpClientExceptionFactory">The factory that can create instances of <see cref="HttpClientException"/>.</param>
        TCustomizer WithCustomHttpClient(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider, 
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory);

        /// <summary>
        /// Sets custom <see cref="ISerializerProvider"/> used to create instances of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        TCustomizer WithCustomSerializer(ISerializerProvider serializerProvider);

        /// <summary>
        /// Sets collection of <see cref="IClientHandler"/> used to handle HTTP requests and responses />.
        /// </summary>
        /// <param name="handlers">The collection of handlers.</param>
        TCustomizer WithCustomHandlers(IReadOnlyCollection<IClientHandler<TRequest, TResponse>> handlers);

        /// <summary>
        /// Sets custom <see cref="IResiliencePolicyProvider"/> used to create instances of <see cref="IResiliencePolicy"/>.
        /// </summary>
        /// <param name="resiliencePolicyProvider">The provider that can create instances of <see cref="IResiliencePolicy"/>.</param>
        TCustomizer WithResiliencePolicy(IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);

        /// <summary>
        /// Sets custom <see cref="IMethodResiliencePolicyProvider"/> used to create instances of <see cref="IResiliencePolicy"/> for specific method.
        /// </summary>
        /// <param name="methodResiliencePolicyProvider">The provider that can create instances of <see cref="IResiliencePolicy"/> for specific method.</param>
        TCustomizer WithResiliencePolicy(IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider);

        /// <summary>
        /// Sets custom <see cref="ILoggerFactory"/> used to create instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="loggerFactory">The factory that can create instances of <see cref="ILogger"/>.</param>
        TCustomizer WithLogging(ILoggerFactory loggerFactory);

        /// <summary>
        /// Creates <see cref="TResult"/>.
        /// </summary>
        TResult Build();
    }
}
