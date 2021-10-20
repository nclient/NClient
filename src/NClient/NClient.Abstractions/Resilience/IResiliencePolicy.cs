using System;
using System.Threading.Tasks;

namespace NClient.Abstractions.Resilience
{
    // TODO: HttpRequest support
    /// <summary>
    /// Transient exception handling policies that can be applied to asynchronous delegates.
    /// </summary>
    public interface IResiliencePolicy<TRequest, TResponse>
    {
        /// <summary>
        /// Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        Task<IResponseContext<TRequest, TResponse>> ExecuteAsync(Func<Task<IResponseContext<TRequest, TResponse>>> action);
    }
}
