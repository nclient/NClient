using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NClient.Providers.Handling;
using NClient.Providers.Results;
using NClient.Providers.Validation;

// ReSharper disable once CheckNamespace
// ReSharper disable UnusedTypeParameter
namespace NClient
{
    public interface INClientOptionalBuilder<TClient, TRequest, TResponse> where TClient : class
    {
        #region ResponseValidation

        INClientOptionalBuilder<TClient, TRequest, TResponse> WithResponseValidation(IResponseValidator<TRequest, TResponse> validator, params IResponseValidator<TRequest, TResponse>[] extraValidators);
        
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithResponseValidation(IEnumerable<IResponseValidator<TRequest, TResponse>> validators);

        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutResponseValidation();

        #endregion

        #region Handling

        INClientOptionalBuilder<TClient, TRequest, TResponse> WithHandling(IClientHandler<TRequest, TResponse> handler, params IClientHandler<TRequest, TResponse>[] extraHandlers);
        
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithHandling(IEnumerable<IClientHandler<TRequest, TResponse>> handlers);
            
        // TODO: doc
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutHandling();
        
        #endregion

        #region Resilience
        
        // TODO: doc
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithResilience(Action<INClientResilienceMethodSelector<TClient, TRequest, TResponse>> configure);
        
        // TODO: doc
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutResilience();
        
        #endregion

        #region Results
        
        // TODO: doc
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomResults(IResultBuilder<TRequest, TResponse> builder, params IResultBuilder<TRequest, TResponse>[] extraBuilders);
        
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomResults(IEnumerable<IResultBuilder<TRequest, TResponse>> builders);

        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutResults();

        #endregion
        
        #region Logging
        
        /// <summary>
        /// Sets custom <see cref="ILoggerFactory"/> used to create instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="loggerFactory">The factory that can create instances of <see cref="ILogger"/>.</param>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithLogging(ILoggerFactory loggerFactory);

        /// <summary>
        /// Sets instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="logger">The logger for a client.</param>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithLogging(ILogger logger, params ILogger[] extraLoggers);
        
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
