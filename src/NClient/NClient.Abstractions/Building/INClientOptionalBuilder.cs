using System;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Configuration.Resilience;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Results;
using NClient.Abstractions.Serialization;

namespace NClient.Abstractions.Building
{
    public interface INClientOptionalBuilder<TClient, TRequest, TResponse> where TClient : class
    {
        #region MyRegion

        public INClientOptionalBuilder<TClient, TRequest, TResponse> EnsuringCustomSuccess(
            params IEnsuringSettings<TRequest, TResponse>[] ensuringSettings);
        
        // TODO: doc
        public INClientOptionalBuilder<TClient, TRequest, TResponse> EnsuringCustomSuccess(
            Predicate<IResponseContext<TRequest, TResponse>> successCondition,
            Action<IResponseContext<TRequest, TResponse>> onFailure);
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> NotEnsuringSuccess();

        #endregion
        
        #region Serializer
        
        /// <summary>
        /// Sets custom <see cref="ISerializerProvider"/> used to create instances of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="serializerProvider">The provider that can create instances of <see cref="ISerializer"/>.</param>
        INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomSerialization(ISerializerProvider serializerProvider);

        #endregion
        
        #region Handling

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
