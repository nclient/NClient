using System.Reflection;

namespace NClient.Abstractions.Resilience
{
    /// <summary>
    /// A provider abstraction for a component that can create <see cref="IResiliencePolicy"/> instances for specific method.
    /// </summary>
    public interface IMethodResiliencePolicyProvider<TResponse>
    {
        /// <summary>
        /// Creates and configures an instance of <see cref="IResiliencePolicy"/> instance.
        /// </summary>
        /// <param name="methodInfo">The method to apply the policy to.</param>
        IResiliencePolicy<TResponse> Create(MethodInfo methodInfo);
    }
}
