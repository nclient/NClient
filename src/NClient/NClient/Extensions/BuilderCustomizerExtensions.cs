using System;
using System.Linq.Expressions;
using System.Net.Http;
using NClient.Abstractions;
using NClient.Abstractions.Resilience;
using NClient.Common.Helpers;
using NClient.Providers.Resilience.Polly;
using Polly;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class BuilderCustomizerExtensions
    {
        /// <summary>
        /// Sets Polly based <see cref="IResiliencePolicyProvider"/> used to create instance of <see cref="IResiliencePolicy"/>.
        /// </summary>
        /// <param name="clientBuilderCustomizer"></param>
        /// <param name="methodSelector">The method to apply the policy to.</param>
        /// <param name="asyncPolicy">The asynchronous policy defining all executions available.</param>
        public static INClientBuilderCustomizer<TInterface, HttpRequestMessage, HttpResponseMessage> WithResiliencePolicy<TInterface>(
            this INClientBuilderCustomizer<TInterface, HttpRequestMessage, HttpResponseMessage> clientBuilderCustomizer,
            Expression<Func<TInterface, Delegate>> methodSelector, IAsyncPolicy<ResponseContext<HttpResponseMessage>> asyncPolicy)
            where TInterface : class
        {
            Ensure.IsNotNull(clientBuilderCustomizer, nameof(clientBuilderCustomizer));
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));

            return clientBuilderCustomizer.WithResiliencePolicy(methodSelector, new PollyResiliencePolicyProvider<HttpResponseMessage>(asyncPolicy));
        }
    }
}
