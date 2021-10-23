using System.Reflection;
using NClient.Providers.Transport;

namespace NClient.Providers.Resilience
{
    // TODO: rename
    // TODO: TRequest support
    /// <summary>
    /// A provider abstraction for a component that can create <see cref="IResiliencePolicy"/> instances for specific method.
    /// </summary>
    public interface IMethodResiliencePolicyProvider<TRequest, TResponse>
    {
        /// <summary>
        /// Creates and configures an instance of <see cref="IResiliencePolicy"/> instance.
        /// </summary>
        /// <param name="methodInfo">The method to apply the policy to.</param>
        /// <param name="httpRequest">The HTTP request to apply the policy to.</param>
        IResiliencePolicy<TRequest, TResponse> Create(MethodInfo methodInfo, IHttpRequest httpRequest);
    }
}
