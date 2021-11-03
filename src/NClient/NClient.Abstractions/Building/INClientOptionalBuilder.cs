using System;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
// ReSharper disable UnusedTypeParameter
namespace NClient
{
    public interface INClientOptionalBuilder<TClient, TRequest, TResponse> where TClient : class
    {
        #region ResponseValidation
        
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutResponseValidation();

        #endregion

        #region Handling
        
        // TODO: doc
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutHandling();
        
        #endregion

        #region Resilience
        
        // TODO: doc
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomResilience(Action<INClientResilienceMethodSelector<TClient, TRequest, TResponse>> configure);
        
        // TODO: doc
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutResilience();
        
        #endregion

        #region Results
        
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutCustomResults();

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
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithLogging(params ILogger[] loggers);
        
        // TODO: doc
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutLogging();
        
        #endregion
        
        /// <summary>
        /// Creates <see cref="TClient"/>.
        /// </summary>
        TClient Build();
    }
}
