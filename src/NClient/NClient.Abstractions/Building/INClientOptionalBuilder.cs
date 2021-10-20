using System;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Configuration.Resilience;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Results;
using NClient.Abstractions.Serialization;
using NClient.Abstractions.Validation;

namespace NClient.Abstractions.Building
{
    public interface INClientOptionalBuilder<TClient, TRequest, TResponse> where TClient : class
    {
        #region Serializer
        
        /// <summary>
        /// Sets custom <see cref="ISerializerProvider"/> used to create instances of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomSerialization(ISerializerProvider serializerProvider);

        #endregion
        
        #region ResponseValidation

        INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomResponseValidation(params IResponseValidatorSettings<TRequest, TResponse>[] responseValidatorSettings);

        INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomResponseValidation(params IResponseValidator<TRequest, TResponse>[] responseValidators);

        INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomResponseValidation(params IResponseValidatorProvider<TRequest, TResponse>[] responseValidatorProvider);

        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutResponseValidation();

        #endregion

        #region Handling

        INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomHandling(params IClientHandlerSettings<TRequest, TResponse>[] clientHandlerSettings);

        INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomHandling(params IClientHandler<TRequest, TResponse>[] handlers);

        INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomHandling(params IClientHandlerProvider<TRequest, TResponse>[] providers);
        
        // TODO: doc
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutHandling();
        
        #endregion

        #region Resilience
        
        /// <summary>
        /// Sets custom <see cref="IMethodResiliencePolicyProvider"/> used to create instances of <see cref="IResiliencePolicy"/> for specific method.
        /// </summary>
        /// <param name="methodResiliencePolicyProvider">The provider that can create instances of <see cref="IResiliencePolicy"/> for specific method.</param>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomResilience(IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider);

        // TODO: doc
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomResilience(Action<INClientResilienceMethodSelector<TClient, TRequest, TResponse>> configure);

        // TODO: doc
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithoutResilience();
        
        #endregion

        #region Results

        // TODO: doc
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomResults(params IResultBuilderProvider<IHttpResponse>[] resultBuilderProviders);
        
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomResults(params IResultBuilderProvider<TResponse>[] resultBuilderProviders);
        
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
