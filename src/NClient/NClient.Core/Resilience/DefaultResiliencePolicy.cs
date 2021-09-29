using System;
using System.Threading.Tasks;
using NClient.Abstractions.Resilience;

namespace NClient.Core.Resilience
{
    internal class DefaultResiliencePolicy<TRequest, TResponse> : IResiliencePolicy<TRequest, TResponse>
    {
        public async Task<TResponse> ExecuteAsync(Func<Task<ResponseContext<TRequest, TResponse>>> action)
        {
            var executionResult = await action.Invoke().ConfigureAwait(false);
            return executionResult.Response;
        }
    }
}
