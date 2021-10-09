using System;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Configuration.Resilience;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Handling;
using NClient.Abstractions.Mapping;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;

namespace NClient.Abstractions.Builders
{
    public interface INClientFactoryOptionalBuilder<TRequest, TResponse>
    {
        #region Ensuring

        public INClientFactoryOptionalBuilder<TRequest, TResponse> EnsuringCustomSuccess(
            IEnsuringSettings<TRequest, TResponse> ensuringSettings);
        
        // TODO: doc
        public INClientFactoryOptionalBuilder<TRequest, TResponse> EnsuringCustomSuccess(
            Predicate<IResponseContext<TRequest, TResponse>> successCondition,
            Action<IResponseContext<TRequest, TResponse>> onFailure);
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> NotEnsuringSuccess();

        #endregion
        
        #region Serializer
        
        /// <summary>
        /// Sets custom <see cref="ISerializerProvider"/> used to create instances of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomSerialization(ISerializerProvider serializerProvider);

        #endregion
        
        #region Responses

        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResponse(
            params IResponseMapper[] responseMappers);
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutCustomResponse();

        #endregion
        
        #region Handling

        /// <summary>
        /// Sets collection of <see cref="IClientHandler{TRequest,TResponse}"/> used to handle HTTP requests and responses />.
        /// </summary>
        /// <param name="handlers">The collection of handlers.</param>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomHandling(params IClientHandler<TRequest, TResponse>[] handlers);
        
        // TODO: doc
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutHandling();
        
        #endregion

        #region Resilience

        // TODO: doc
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithForceResilience(IResiliencePolicyProvider<TRequest, TResponse> provider);
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithIdempotentResilience(IResiliencePolicyProvider<TRequest, TResponse> idempotentMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider);
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithSafeResilience(IResiliencePolicyProvider<TRequest, TResponse> safeMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider);

        /// <summary>
        /// Sets custom <see cref="IMethodResiliencePolicyProvider"/> used to create instances of <see cref="IResiliencePolicy"/> for specific method.
        /// </summary>
        /// <param name="methodResiliencePolicyProvider">The provider that can create instances of <see cref="IResiliencePolicy"/> for specific method.</param>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResilience(IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider);

        /// <summary>
        /// Sets custom <see cref="IResiliencePolicyProvider{TRequest,TResponse}"/> used to create instances of <see cref="IResiliencePolicy"/> for specific method.
        /// </summary>
        /// <param name="configure"></param>
        // TODO: doc
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResilience(Action<INClientFactoryResilienceMethodSelector<TRequest, TResponse>> configure);

        // TODO: doc
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutResilience();
        
        #endregion
        
        #region Logging
        
        /// <summary>
        /// Sets custom <see cref="ILoggerFactory"/> used to create instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="loggerFactory">The factory that can create instances of <see cref="ILogger"/>.</param>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithLogging(ILoggerFactory loggerFactory);

        /// <summary>
        /// Sets instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="logger">The logger for a client.</param>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithLogging(ILogger logger);
        
        // TODO: doc
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutLogging();
        
        #endregion
        
        /// <summary>
        /// Creates <see cref="INClientFactory"/>.
        /// </summary>
        INClientFactory Build();
    }
}
