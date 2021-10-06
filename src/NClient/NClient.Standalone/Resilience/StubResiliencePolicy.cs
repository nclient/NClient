using System;
using System.Threading.Tasks;
using NClient.Abstractions.Resilience;

namespace NClient.Standalone.Resilience
{
    internal class StubResiliencePolicy<TRequest, TResponse> : IResiliencePolicy<TRequest, TResponse>
    {
        public async Task<TResponse> ExecuteAsync(Func<Task<ResponseContext<TRequest, TResponse>>> action)
        {
            var responseContext = await action.Invoke().ConfigureAwait(false);
            return responseContext.Response;
        }
    }
}
