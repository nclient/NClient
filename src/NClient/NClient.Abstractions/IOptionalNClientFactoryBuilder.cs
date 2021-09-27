using System;
using System.Linq.Expressions;
using NClient.Abstractions.Resilience;

namespace NClient.Abstractions
{
    public interface IOptionalNClientFactoryBuilder
        : IOptionalBuilderBase<IOptionalNClientFactoryBuilder, INClientFactory>
    {
        /// <summary>
        /// Sets custom <see cref="IResiliencePolicyProvider"/> used to create instances of <see cref="IResiliencePolicy"/> for specific method.
        /// </summary>
        /// <param name="methodSelector">The method to apply the policy to.</param>
        /// <param name="resiliencePolicyProvider">The provider that can create instances of <see cref="IResiliencePolicy"/>.</param>
        /// <typeparam name="TInterface">The type of client interface.</typeparam>
        IOptionalNClientFactoryBuilder WithResiliencePolicy<TInterface>(Expression<Func<TInterface, Delegate>> methodSelector, IResiliencePolicyProvider resiliencePolicyProvider);
    }
}
