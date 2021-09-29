using System;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Resilience;

namespace NClient.Abstractions
{
    public interface INClientBuilderCustomizer<TInterface, TRequest, TResponse>
        : INClientCommonCustomizer<INClientBuilderCustomizer<TInterface, TRequest, TResponse>, TInterface, TRequest, TResponse>
        where TInterface : class
    {
        /// <summary>
        /// Sets custom <see cref="IResiliencePolicyProvider"/> used to create instances of <see cref="IResiliencePolicy"/> for specific method.
        /// </summary>
        /// <param name="methodSelector">The method to apply the policy to.</param>
        /// <param name="resiliencePolicyProvider">The provider that can create instances of <see cref="IResiliencePolicy"/>.</param>
        INClientBuilderCustomizer<TInterface, TRequest, TResponse> WithResiliencePolicy(Expression<Func<TInterface, Delegate>> methodSelector, IResiliencePolicyProvider<TRequest, TResponse> resiliencePolicyProvider);

        /// <summary>
        /// Sets custom <see cref="ILoggerFactory"/> used to create instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="logger">The logger for a client.</param>
        INClientBuilderCustomizer<TInterface, TRequest, TResponse> WithLogging(ILogger<TInterface> logger);
    }
}
