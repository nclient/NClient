using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NClient.Providers.Authorization;
using NClient.Providers.Handling;
using NClient.Providers.Mapping;
using NClient.Providers.Serialization;
using NClient.Providers.Validation;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>The builder that provides the ability to set optional settings.</summary>
    /// <typeparam name="TClient">The type of client interface.</typeparam>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface INClientOptionalBuilder<TClient, TRequest, TResponse> where TClient : class
    {
        #region Authorization

        /// <summary>Sets access tokens for authorization.</summary>
        /// <param name="accessTokens">The access tokens for client authorization.</param>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithTokenAuthorization(IAccessTokens accessTokens);

        /// <summary>Removes authorization tokens.</summary>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutAuthorization();
        
        #endregion
        
        #region Serializer
        
        /// <summary>Sets custom <see cref="ISerializerProvider"/> used to create instances of <see cref="ISerializer"/>.</summary>
        /// <param name="provider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomSerialization(ISerializerProvider provider);

        #endregion
        
        #region ResponseValidation
        
        /// <summary>Sets response validation of the contents received from transport.</summary>
        /// <param name="validators">The collection of response validators for validation the contents of the response received from transport.</param>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithResponseValidation(IEnumerable<IResponseValidator<TRequest, TResponse>> validators);

        /// <summary>Sets advanced response validation of the contents received from transport.</summary>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithAdvancedResponseValidation(Action<INClientResponseValidationSelector<TRequest, TResponse>> configure);

        /// <summary>Removes response validation of the contents received from transport.</summary>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutResponseValidation();

        #endregion

        #region Handling
        
        /// <summary>Sets handling operations that handles the transport messages.</summary>
        /// <param name="handlers">The collection of handlers that provides custom functionality to handling transport requests and responses.</param>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithHandling(IEnumerable<IClientHandler<TRequest, TResponse>> handlers);
        
        /// <summary>Sets advanced handling operations that handles the transport messages.</summary>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithAdvancedHandling(Action<INClientHandlingSelector<TRequest, TResponse>> configure);

        /// <summary>Removes handling operations that handles the transport messages.</summary>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutHandling();
        
        #endregion

        #region Mapping
        
        /// <summary>Sets mapping that convert NClient responses into custom results.</summary>
        /// <param name="mappers">The collection of mappers that converts transport responses into custom results.</param>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithResponseMapping(IEnumerable<IResponseMapper<TRequest, TResponse>> mappers);

        /// <summary>Sets advanced mapping that convert NClient responses into custom results.</summary>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithAdvancedResponseMapping(Action<INClientResponseMappingSelector<TRequest, TResponse>> configure);

        /// <summary>Removes mapping that convert NClient responses into custom results.</summary>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutResponseMapping();

        #endregion
        
        #region Resilience
        
        /// <summary>Sets a resilience policy for specific method/methods.</summary>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithResilience(Action<INClientResilienceMethodSelector<TClient, TRequest, TResponse>> configure);
        
        /// <summary>Removes a resilience policy for all methods.</summary>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutResilience();
        
        #endregion

        #region Timeout

        /// <summary>Set a timeout for all client methods.</summary>
        /// <param name="timeout">The timeout in milliseconds.</param>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithTimeout(TimeSpan timeout);

        #endregion
        
        #region Logging
        
        /// <summary>Sets a logger factory for logging client actions.</summary>
        /// <param name="loggerFactory">The factory that can create instances of <see cref="ILogger"/>.</param>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithLogging(ILoggerFactory loggerFactory);
        
        /// <summary>Sets a logger for logging client actions.</summary>
        /// <param name="loggers">The collection of loggers.</param>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithLogging(IEnumerable<ILogger> loggers);
        
        /// <summary>Removes logging of client actions.</summary>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutLogging();
        
        #endregion

        /// <summary>Creates instance of <see cref="TClient"/>.</summary>
        TClient Build();
    }
}
