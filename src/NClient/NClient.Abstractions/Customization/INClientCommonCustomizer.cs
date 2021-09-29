using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;

namespace NClient.Abstractions.Customization
{
    public interface INClientCommonCustomizer<TConcreateCustomizer, TResult, TRequest, TResponse>
        where TConcreateCustomizer : INClientCommonCustomizer<TConcreateCustomizer, TResult, TRequest, TResponse>
    {
        /// <summary>
        /// Sets custom <see cref="IHttpClientProvider"/> used to create instances of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="httpClientProvider">The provider that can create instances of <see cref="IHttpClient"/>.</param>
        /// <param name="httpMessageBuilderProvider">The provider that can create instances of <see cref="IHttpMessageBuilder"/>.</param>
        /// <param name="httpClientExceptionFactory">The factory that can create instances of <see cref="HttpClientException"/>.</param>
        TConcreateCustomizer UsingCustomHttpClient(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider, 
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory);

        /// <summary>
        /// Sets custom <see cref="ISerializerProvider"/> used to create instances of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        TConcreateCustomizer UsingCustomSerializer(ISerializerProvider serializerProvider);

        /// <summary>
        /// Sets collection of <see cref="IClientHandler"/> used to handle HTTP requests and responses />.
        /// </summary>
        /// <param name="handlers">The collection of handlers.</param>
        TConcreateCustomizer WithCustomHandling(IReadOnlyCollection<IClientHandler<TRequest, TResponse>> handlers);
        
        // TODO: doc
        TConcreateCustomizer WithoutHandling();
        
        // TODO: doc
        TConcreateCustomizer WithForceResilience(IResiliencePolicyProvider<TRequest, TResponse> provider);
        TConcreateCustomizer WithIdempotentResilience(IResiliencePolicyProvider<TRequest, TResponse> idempotentMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider);
        TConcreateCustomizer WithSafeResilience(IResiliencePolicyProvider<TRequest, TResponse> safeMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider);

        /// <summary>
        /// Sets custom <see cref="IMethodResiliencePolicyProvider"/> used to create instances of <see cref="IResiliencePolicy"/> for specific method.
        /// </summary>
        /// <param name="methodResiliencePolicyProvider">The provider that can create instances of <see cref="IResiliencePolicy"/> for specific method.</param>
        TConcreateCustomizer WithCustomResilience(IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider);

        // TODO: doc
        TConcreateCustomizer WithoutResilience();
        
        /// <summary>
        /// Sets custom <see cref="ILoggerFactory"/> used to create instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="loggerFactory">The factory that can create instances of <see cref="ILogger"/>.</param>
        TConcreateCustomizer WithLogging(ILoggerFactory loggerFactory);
        
        /// <summary>
        /// Sets instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="logger">The logger for a client.</param>
        TConcreateCustomizer WithLogging(ILogger logger);
        
        // TODO: doc
        TConcreateCustomizer WithoutLogging();

        /// <summary>
        /// Creates <see cref="TResult"/>.
        /// </summary>
        TResult Build();
    }
}
