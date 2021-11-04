using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NClient.Providers.Serialization;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse>
    {
        #region Serializer
        
        /// <summary>
        /// Sets custom <see cref="ISerializerProvider"/> used to create instances of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="provider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithCustomSerialization(ISerializerProvider provider);

        #endregion
        
        #region ResponseValidation

        INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithResponseValidation(Action<INClientResponseValidationSelector<TRequest, TResponse>> configure);
        
        INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithoutResponseValidation();

        #endregion

        #region Handling

        INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithHandling(Action<INClientHandlingSelector<TRequest, TResponse>> configure);
        
        // TODO: doc
        INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithoutHandling();
        
        #endregion

        #region Results
        
        // TODO: doc
        INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithResults(Action<INClientResultsSelector<TRequest, TResponse>> configure);
        
        INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithoutResults();

        #endregion
        
        #region Resilience
        
        // TODO: doc
        INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithResilience(Action<INClientFactoryResilienceMethodSelector<TRequest, TResponse>> configure);

        // TODO: doc
        INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithoutResilience();
        
        #endregion
        
        #region Logging
        
        /// <summary>
        /// Sets custom <see cref="ILoggerFactory"/> used to create instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="loggerFactory">The factory that can create instances of <see cref="ILogger"/>.</param>
        // Not advanced
        INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithLogging(ILoggerFactory loggerFactory);
        
        INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithLogging(IEnumerable<ILogger> loggers);
        
        // TODO: doc
        INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithoutLogging();
        
        #endregion

        /// <summary>
        /// Creates instance of <see cref="INClientFactory"/>.
        /// </summary>
        INClientFactory Build();
    }
}
