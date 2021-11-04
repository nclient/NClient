﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NClient.Providers.Resilience;
using NClient.Providers.Serialization;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> where TClient : class
    {
        #region Serializer
        
        /// <summary>
        /// Sets custom <see cref="ISerializerProvider"/> used to create instances of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="provider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithCustomSerialization(ISerializerProvider provider);

        #endregion
        
        #region ResponseValidation

        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithResponseValidation(Action<INClientAdvancedResponseValidationSetter<TRequest, TResponse>> configure);

        // Not advanced
        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithoutResponseValidation();

        #endregion

        #region Handling

        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithHandling(Action<INClientAdvancedHandlingSetter<TRequest, TResponse>> configure);
        
        // TODO: doc
        // Not advanced
        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithoutHandling();
        
        #endregion

        #region Resilience
        
        /// <summary>
        /// Sets custom <see cref="IMethodResiliencePolicyProvider{TRequest,TResponse}"/> used to create instances of <see cref="IResiliencePolicy{TRequest,TResponse}"/> for specific method.
        /// </summary>
        /// <param name="provider">The provider that can create instances of <see cref="IResiliencePolicy{TRequest,TResponse}"/> for specific method.</param>
        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithCustomResilience(IMethodResiliencePolicyProvider<TRequest, TResponse> provider);

        // TODO: doc
        // Not advanced
        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithResilience(Action<INClientResilienceMethodSelector<TClient, TRequest, TResponse>> configure);

        // TODO: doc
        // Not advanced
        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithoutResilience();
        
        #endregion

        #region Results
        
        // TODO: doc
        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithCustomResults(Action<INClientAdvancedResultsSetter<TRequest, TResponse>> configure);

        // Not advanced
        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithoutResults();

        #endregion
        
        #region Logging
        
        /// <summary>
        /// Sets custom <see cref="ILoggerFactory"/> used to create instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="loggerFactory">The factory that can create instances of <see cref="ILogger"/>.</param>
        // Not advanced
        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithLogging(ILoggerFactory loggerFactory);

        /// <summary>
        /// Sets instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="logger">The logger for a client.</param>
        // Not advanced
        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithLogging(ILogger logger, params ILogger[] extraLoggers);
        
        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithLogging(IEnumerable<ILogger> loggers);
        
        // TODO: doc
        // Not advanced
        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithoutLogging();
        
        #endregion

        /// <summary>
        /// Creates <see cref="TClient"/>.
        /// </summary>
        // Not advanced
        TClient Build();
    }
}
