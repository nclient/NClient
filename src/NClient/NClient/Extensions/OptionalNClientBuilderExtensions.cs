using System;
using System.Linq.Expressions;
using NClient.Abstractions;
using NClient.Abstractions.Resilience;
using NClient.Common.Helpers;
using NClient.Providers.Resilience.Polly;
using Polly;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class OptionalNClientBuilderExtensions
    {
        /// <summary>
        /// Sets Polly based <see cref="IResiliencePolicyProvider"/> used to create instance of <see cref="IResiliencePolicy"/>.
        /// </summary>
        /// <param name="clientBuilder"></param>
        /// <param name="methodSelector">The method to apply the policy to.</param>
        /// <param name="asyncPolicy">The asynchronous policy defining all executions available.</param>
        public static IOptionalNClientBuilder<TInterface> WithResiliencePolicy<TInterface>(
            this IOptionalNClientBuilder<TInterface> clientBuilder,
            Expression<Func<TInterface, Delegate>> methodSelector, IAsyncPolicy<ResponseContext> asyncPolicy)
            where TInterface : class
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));

            return clientBuilder.WithResiliencePolicy(methodSelector, new PollyResiliencePolicyProvider(asyncPolicy));
        }
    }
}
