﻿using System;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Resilience;

namespace NClient.Abstractions
{
    public interface IOptionalNClientBuilder<TInterface>
        : IOptionalBuilderBase<IOptionalNClientBuilder<TInterface>, TInterface>
        where TInterface : class
    {
        /// <summary>
        /// Sets custom <see cref="IResiliencePolicyProvider"/> used to create instances of <see cref="IResiliencePolicy"/> for specific method.
        /// </summary>
        /// <param name="methodSelector">The method to apply the policy to.</param>
        /// <param name="resiliencePolicyProvider">The provider that can create instances of <see cref="IResiliencePolicy"/>.</param>
        IOptionalNClientBuilder<TInterface> WithResiliencePolicy(Expression<Func<TInterface, Delegate>> methodSelector, IResiliencePolicyProvider resiliencePolicyProvider);

        /// <summary>
        /// Sets custom <see cref="ILoggerFactory"/> used to create instances of <see cref="ILogger"/>.
        /// </summary>
        /// <param name="logger">The logger for a client.</param>
        IOptionalNClientBuilder<TInterface> WithLogging(ILogger<TInterface> logger);
    }
}