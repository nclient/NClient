using System;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;

namespace NClient.Abstractions.Resilience
{
    /// <summary>
    /// Transient exception handling policies that can be applied to asynchronous delegates.
    /// </summary>
    public interface IResiliencePolicy
    {
        /// <summary>
        /// Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        Task<HttpResponse> ExecuteAsync(Func<Task<ResponseContext>> action);
    }
}
