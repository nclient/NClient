using System;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;

namespace NClient.Core.Resilience
{
    internal class StubResiliencePolicy : IResiliencePolicy
    {
        public Task<HttpResponse> ExecuteAsync(Func<Task<HttpResponse>> action, string policyKey)
        {
            return action.Invoke();
        }
    }
}
