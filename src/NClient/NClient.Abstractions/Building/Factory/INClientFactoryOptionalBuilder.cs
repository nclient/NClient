using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NClient.Providers.Handling;
using NClient.Providers.Results;
using NClient.Providers.Serialization;
using NClient.Providers.Validation;

// ReSharper disable once CheckNamespace
// ReSharper disable UnusedTypeParameter
namespace NClient
{
    public interface INClientFactoryOptionalBuilder<TRequest, TResponse>
    {
        #region Serializer
        
        /// <summary>
        /// Sets custom <see cref="ISerializerProvider"/> used to create instances of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="provider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomSerialization(ISerializerProvider provider);

        #endregion
        
        #region ResponseValidation
        
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithResponseValidation(IEnumerable<IResponseValidator<TRequest, TResponse>> validators);

        INClientFactoryOptionalBuilder<TRequest, TResponse> WithAdvancedResponseValidation(Action<INClientResponseValidationSelector<TRequest, TResponse>> configure);

        INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutResponseValidation();

        #endregion

        #region Handling

        INClientFactoryOptionalBuilder<TRequest, TResponse> WithHandling(IEnumerable<IClientHandler<TRequest, TResponse>> handlers);
        
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithAdvancedHandling(Action<INClientHandlingSelector<TRequest, TResponse>> configure);

        // TODO: doc
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutHandling();
        
        #endregion

        #region Results
        
        // TODO: doc
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithResults(IEnumerable<IResultBuilder<TRequest, TResponse>> builders);

        // TODO: doc
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithAdvancedResults(Action<INClientResultsSelector<TRequest, TResponse>> configure);

        INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutResults();

        #endregion
        
        #region Resilience
        
        // TODO: doc
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithResilience(Action<INClientFactoryResilienceMethodSelector<TRequest, TResponse>> configure);
        
        // TODO: doc
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutResilience();
        
        #endregion
        
        #region Logging
        
        /// <summary>
        /// Sets custom <see cref="ILoggerFactory"/> used to create instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="loggerFactory">The factory that can create instances of <see cref="ILogger"/>.</param>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithLogging(ILoggerFactory loggerFactory);
        
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithLogging(IEnumerable<ILogger> loggers);
        
        // TODO: doc
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutLogging();
        
        #endregion
        
        /// <summary>
        /// Creates instance of <see cref="INClientFactory"/>.
        /// </summary>
        INClientFactory Build();
    }
}
