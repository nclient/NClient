using System;
using System.Threading.Tasks;
using NClient.Abstractions.Providers.Resilience;

namespace NClient.Standalone.Client.Resilience
{
    internal class StubResiliencePolicy<TRequest, TResponse> : IResiliencePolicy<TRequest, TResponse>
    {
        public async Task<IResponseContext<TRequest, TResponse>> ExecuteAsync(Func<Task<IResponseContext<TRequest, TResponse>>> action)
        {
            return await action.Invoke().ConfigureAwait(false);
        }
    }
}
