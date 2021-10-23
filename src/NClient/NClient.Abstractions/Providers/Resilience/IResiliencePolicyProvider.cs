// ReSharper disable once CheckNamespace

namespace NClient.Providers.Resilience
{
    /// <summary>
    /// A provider abstraction for a component that can create <see cref="IResiliencePolicy"/> instances.
    /// </summary>
    public interface IResiliencePolicyProvider<TRequest, TResponse>
    {
        /// <summary>
        /// Creates and configures an instance of <see cref="IResiliencePolicy"/> instance.
        /// </summary>
        IResiliencePolicy<TRequest, TResponse> Create();
    }
}
