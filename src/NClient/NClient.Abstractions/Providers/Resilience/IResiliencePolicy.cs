using System;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Transport;

namespace NClient.Providers.Resilience
{
    /// <summary>Transient exception handling policy.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface IResiliencePolicy<TRequest, TResponse>
    {
        /// <summary>Executes the specified asynchronous action within the policy and returns the result.</summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="cancellationToken">The cancellation token to terminate the policy.</param>
        Task<IResponseContext<TRequest, TResponse>> ExecuteAsync(
            Func<CancellationToken, Task<IResponseContext<TRequest, TResponse>>> action, CancellationToken cancellationToken);
    }
}
