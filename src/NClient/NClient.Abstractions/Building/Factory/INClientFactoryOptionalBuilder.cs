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
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface INClientFactoryOptionalBuilder<TRequest, TResponse>
    {
        #region Authorization
        
        /// <summary>Sets access tokens for authorization.</summary>
        /// <param name="accessTokens">The access tokens for client authorization.</param>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithTokenAuthorization(IAccessTokens accessTokens);

        /// <summary>Removes authorization tokens.</summary>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutAuthorization();
        
        #endregion
        
        #region Serializer
        
        /// <summary>Sets custom <see cref="ISerializerProvider"/> used to create instances of <see cref="ISerializer"/>.</summary>
        /// <param name="provider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomSerialization(ISerializerProvider provider);

        #endregion
        
        #region ResponseValidation
        
        /// <summary>Sets response validation of the contents received from transport.</summary>
        /// <param name="validators">The collection of response validators for validation the contents of the response received from transport.</param>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithResponseValidation(IEnumerable<IResponseValidator<TRequest, TResponse>> validators);

        /// <summary>Sets advanced response validation of the contents received from transport.</summary>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithAdvancedResponseValidation(Action<INClientResponseValidationSelector<TRequest, TResponse>> configure);

        /// <summary>Removes response validation of the contents received from transport.</summary>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutResponseValidation();

        #endregion

        #region Handling

        /// <summary>Sets handling operations that handles the transport messages.</summary>
        /// <param name="handlers">The collection of handlers that provides custom functionality to handling transport requests and responses.</param>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithHandling(IEnumerable<IClientHandler<TRequest, TResponse>> handlers);
        
        /// <summary>Sets advanced handling operations that handles the transport messages.</summary>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithAdvancedHandling(Action<INClientHandlingSelector<TRequest, TResponse>> configure);
        
        /// <summary>Removes handling operations that handles the transport messages.</summary>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutHandling();
        
        #endregion

        #region Results
        
        /// <summary>Sets mapping that convert NClient responses into custom results.</summary>
        /// <param name="mappers">The collection of mappers that converts transport responses into custom results.</param>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithResponseMapping(IEnumerable<IResponseMapper<TRequest, TResponse>> mappers);
        
        /// <summary>Sets advanced mapping that convert NClient responses into custom results.</summary>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithAdvancedResponseMapping(Action<INClientResponseMappingSelector<TRequest, TResponse>> configure);

        /// <summary>Removes mapping that convert NClient responses into custom results.</summary>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutResponseMapping();

        #endregion
        
        #region Resilience
        
        /// <summary>Sets a resilience policy for specific method/methods.</summary>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithResilience(Action<INClientFactoryResilienceMethodSelector<TRequest, TResponse>> configure);
        
        /// <summary>Removes a resilience policy for all methods.</summary>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutResilience();
        
        #endregion
        
        #region Timeout

        /// <summary>Set a timeout for all client methods.</summary>
        /// <param name="timeout">The timeout in milliseconds.</param>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithTimeout(TimeSpan timeout);

        #endregion
        
        #region Logging
        
        /// <summary>Sets a logger factory for logging client actions.</summary>
        /// <param name="loggerFactory">The factory that can create instances of <see cref="ILogger"/>.</param>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithLogging(ILoggerFactory loggerFactory);

        /// <summary>Sets a logger for logging client actions.</summary>
        /// <param name="loggers">The collection of loggers.</param>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithLogging(IEnumerable<ILogger> loggers);
        
        /// <summary>Removes logging of client actions.</summary>
        INClientFactoryOptionalBuilder<TRequest, TResponse> WithoutLogging();
        
        #endregion
        
        /// <summary>Creates instance of <see cref="INClientFactory"/>.</summary>
        INClientFactory Build();
    }
}
