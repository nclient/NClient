using System;
using System.Threading.Tasks;
using NClient.Providers.Resilience;

namespace NClient.Standalone.ClientProxy.Validation.Resilience
{
    internal class StubResiliencePolicy<TRequest, TResponse> : IResiliencePolicy<TRequest, TResponse>
    {
        public async Task<IResponseContext<TRequest, TResponse>> ExecuteAsync(Func<Task<IResponseContext<TRequest, TResponse>>> action)
        {
            return await action.Invoke().ConfigureAwait(false);
        }
    }
}
