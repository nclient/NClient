using System;
using System.Threading.Tasks;
using NClient.Abstractions.Resilience;

namespace NClient.Core.Resilience
{
    internal class StubResiliencePolicy : IResiliencePolicy
    {
        public Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action)
        {
            return action.Invoke();
        }
    }
}
