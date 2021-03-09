using System;
using System.Threading.Tasks;
using NClient.Providers.Resilience.Abstractions;
using Polly;

namespace NClient.Providers.Resilience.Polly
{
    public class PollyResiliencePolicy : IResiliencePolicy
    {
        private readonly IAsyncPolicy _asyncPolicy;

        public PollyResiliencePolicy(IAsyncPolicy asyncPolicy)
        {
            _asyncPolicy = asyncPolicy;
        }
        public Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action)
        {
            return _asyncPolicy.ExecuteAsync(action);
        }
    }
}
