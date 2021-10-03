using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;

namespace NClient.Abstractions.Customization
{
    public interface INClientCommonCustomizer<TSpecificCustomizer, TRequest, TResponse>
    {
        /// <summary>
        /// Sets custom <see cref="IHttpClientProvider"/> used to create instances of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="httpClientProvider">The provider that can create instances of <see cref="IHttpClient"/>.</param>
        /// <param name="httpMessageBuilderProvider">The provider that can create instances of <see cref="IHttpMessageBuilder"/>.</param>
        /// <param name="httpClientExceptionFactory">The factory that can create instances of <see cref="HttpClientException"/>.</param>
        TSpecificCustomizer UsingCustomHttpClient(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider, 
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory);

        /// <summary>
        /// Sets custom <see cref="ISerializerProvider"/> used to create instances of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        TSpecificCustomizer UsingCustomSerializer(ISerializerProvider serializerProvider);

        /// <summary>
        /// Sets collection of <see cref="IClientHandler"/> used to handle HTTP requests and responses />.
        /// </summary>
        /// <param name="handlers">The collection of handlers.</param>
        TSpecificCustomizer WithCustomHandling(IReadOnlyCollection<IClientHandler<TRequest, TResponse>> handlers);
        
        // TODO: doc
        TSpecificCustomizer WithoutHandling();
        
        // TODO: doc
        TSpecificCustomizer WithForceResilience(IResiliencePolicyProvider<TRequest, TResponse> provider);
        TSpecificCustomizer WithIdempotentResilience(IResiliencePolicyProvider<TRequest, TResponse> idempotentMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider);
        TSpecificCustomizer WithSafeResilience(IResiliencePolicyProvider<TRequest, TResponse> safeMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider);

        /// <summary>
        /// Sets custom <see cref="IMethodResiliencePolicyProvider"/> used to create instances of <see cref="IResiliencePolicy"/> for specific method.
        /// </summary>
        /// <param name="methodResiliencePolicyProvider">The provider that can create instances of <see cref="IResiliencePolicy"/> for specific method.</param>
        TSpecificCustomizer WithCustomResilience(IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider);

        // TODO: doc
        TSpecificCustomizer WithoutResilience();
        
        /// <summary>
        /// Sets custom <see cref="ILoggerFactory"/> used to create instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="loggerFactory">The factory that can create instances of <see cref="ILogger"/>.</param>
        TSpecificCustomizer WithLogging(ILoggerFactory loggerFactory);
        
        /// <summary>
        /// Sets instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="logger">The logger for a client.</param>
        TSpecificCustomizer WithLogging(ILogger logger);
        
        // TODO: doc
        TSpecificCustomizer WithoutLogging();
    }
}
