using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NClient.Providers.Handling;
using NClient.Providers.Mapping;
using NClient.Providers.Serialization;
using NClient.Providers.Validation;

// ReSharper disable once CheckNamespace
// ReSharper disable UnusedTypeParameter
namespace NClient
{
    public interface INClientOptionalBuilder<TClient, TRequest, TResponse> where TClient : class
    {
        #region Serializer
        
        /// <summary>
        /// Sets custom <see cref="ISerializerProvider"/> used to create instances of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="provider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomSerialization(ISerializerProvider provider);

        #endregion
        
        #region ResponseValidation
        
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithResponseValidation(IEnumerable<IResponseValidator<TRequest, TResponse>> validators);

        INClientOptionalBuilder<TClient, TRequest, TResponse> WithAdvancedResponseValidation(Action<INClientResponseValidationSelector<TRequest, TResponse>> configure);

        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutResponseValidation();

        #endregion

        #region Handling
        
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithHandling(IEnumerable<IClientHandler<TRequest, TResponse>> handlers);
        
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithAdvancedHandling(Action<INClientHandlingSelector<TRequest, TResponse>> configure);

        // TODO: doc
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutHandling();
        
        #endregion

        #region Results
        
        // TODO: doc
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithResponseMapping(IEnumerable<IResponseMapper<TRequest, TResponse>> builders);

        INClientOptionalBuilder<TClient, TRequest, TResponse> WithAdvancedResponseMapping(Action<INClientResponseMappingSelector<TRequest, TResponse>> configure);

        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutResponseMapping();

        #endregion
        
        #region Resilience
        
        // TODO: doc
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithResilience(Action<INClientResilienceMethodSelector<TClient, TRequest, TResponse>> configure);
        
        // TODO: doc
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutResilience();
        
        #endregion
        
        #region Logging
        
        /// <summary>
        /// Sets custom <see cref="ILoggerFactory"/> used to create instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="loggerFactory">The factory that can create instances of <see cref="ILogger"/>.</param>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithLogging(ILoggerFactory loggerFactory);
        
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithLogging(IEnumerable<ILogger> loggers);
        
        // TODO: doc
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutLogging();
        
        #endregion

        /// <summary>
        /// Creates <see cref="TClient"/>.
        /// </summary>
        TClient Build();
    }
}
