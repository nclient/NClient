using System;
using Microsoft.Extensions.Logging;
using NClient.Providers.Resilience;
using NClient.Providers.Results;
using NClient.Providers.Serialization;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> where TClient : class
    {
        #region Serializer
        
        /// <summary>
        /// Sets custom <see cref="ISerializerProvider"/> used to create instances of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithCustomSerialization(ISerializerProvider serializerProvider);

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
        /// <param name="methodResiliencePolicyProvider">The provider that can create instances of <see cref="IResiliencePolicy{TRequest,TResponse}"/> for specific method.</param>
        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithCustomResilience(IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider);

        // TODO: doc
        // Not advanced
        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithResilience(Action<INClientResilienceMethodSelector<TClient, TRequest, TResponse>> configure);

        // TODO: doc
        // Not advanced
        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithoutResilience();
        
        #endregion

        #region Results

        // TODO: doc
        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithCustomResults(params IResultBuilderProvider<IRequest, IResponse>[] resultBuilderProviders);
        
        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithCustomResults(params IResultBuilderProvider<TRequest, TResponse>[] resultBuilderProviders);
        
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
        INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithLogging(params ILogger[] loggers);
        
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
