using System;
using System.Threading.Tasks;

namespace NClient.Abstractions.Resilience.Providers
{
    internal class NoResiliencePolicy<TRequest, TResponse> : IResiliencePolicy<TRequest, TResponse>
    {
        public async Task<TResponse> ExecuteAsync(Func<Task<ResponseContext<TRequest, TResponse>>> action)
        {
            var responseContext = await action.Invoke().ConfigureAwait(false);
            return responseContext.Response;
        }
    }
}
