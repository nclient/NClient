using System;
using Microsoft.Extensions.Logging;
using NClient.Providers.Handling;
using NClient.Providers.Resilience;
using NClient.Providers.Results;
using NClient.Providers.Serialization;
using NClient.Providers.Transport;
using NClient.Providers.Validation;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientFactoryOptionalBuilder<TRequest, TResponse>
    {
        #region Serializer
        
        /// <summary>
        /// Sets custom <see cref="ISerializerProvider"/> used to create instances of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomSerialization(ISerializerProvider serializerProvider);

        #endregion
        
        #region ResponseValidation

        INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResponseValidation(params IResponseValidatorSettings<TRequest, TResponse>[] responseValidatorSettings);

        INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResponseValidation(params IResponseValidator<TRequest, TResponse>[] responseValidators);

        INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResponseValidation(params IResponseValidatorProvider<TRequest, TResponse>[] responseValidatorProviders);

        INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutResponseValidation();

        #endregion

        #region Handling

        INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomHandling(params IClientHandlerSettings<TRequest, TResponse>[] clientHandlerSettings);

        INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomHandling(params IClientHandler<TRequest, TResponse>[] handlers);
        
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomHandling(params IClientHandlerProvider<TRequest, TResponse>[] providers);

        // TODO: doc
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutHandling();
        
        #endregion

        #region Resilience
        
        /// <summary>
        /// Sets custom <see cref="IMethodResiliencePolicyProvider{TRequest,TResponse}"/> used to create instances of <see cref="IResiliencePolicy{TRequest,TResponse}"/> for specific method.
        /// </summary>
        /// <param name="methodResiliencePolicyProvider">The provider that can create instances of <see cref="IResiliencePolicy{TRequest,TResponse}"/> for specific method.</param>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResilience(IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider);

        /// <summary>
        /// Sets custom <see cref="IResiliencePolicyProvider{TRequest,TResponse}"/> used to create instances of <see cref="IResiliencePolicy{TRequest,TResponse}"/> for specific method.
        /// </summary>
        /// <param name="configure"></param>
        // TODO: doc
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResilience(Action<INClientFactoryResilienceMethodSelector<TRequest, TResponse>> configure);

        // TODO: doc
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutResilience();
        
        #endregion
        
        #region Results

        // TODO: doc
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResults(params IResultBuilderProvider<IRequest, IResponse>[] resultBuilderProviders);
        
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomResults(params IResultBuilderProvider<TRequest, TResponse>[] resultBuilderProviders);
        
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutCustomResults();

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
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithLogging(params ILogger[] loggers);
        
        // TODO: doc
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutLogging();
        
        #endregion
        
        /// <summary>
        /// Creates <see cref="INClientFactory"/>.
        /// </summary>
        INClientFactory Build();
    }
}
