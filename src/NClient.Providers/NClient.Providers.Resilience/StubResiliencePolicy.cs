using System;
using System.Threading.Tasks;
using NClient.Providers.Resilience.Abstractions;

namespace NClient.Providers.Resilience
{
    public class StubResiliencePolicy : IResiliencePolicy
    {
        public Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action)
        {
            return action.Invoke();
        }
    }
}
