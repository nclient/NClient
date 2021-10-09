using System;
using System.Threading.Tasks;
using NClient.Abstractions.Resilience;

namespace NClient.Standalone.Resilience
{
    internal class StubResiliencePolicy<TRequest, TResponse> : IResiliencePolicy<TRequest, TResponse>
    {
        public async Task<IResponseContext<TRequest, TResponse>> ExecuteAsync(Func<Task<IResponseContext<TRequest, TResponse>>> action)
        {
            return await action.Invoke().ConfigureAwait(false);
        }
    }
}
