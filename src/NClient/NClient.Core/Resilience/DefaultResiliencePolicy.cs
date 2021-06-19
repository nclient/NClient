using System;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;

namespace NClient.Core.Resilience
{
    internal class DefaultResiliencePolicy : IResiliencePolicy
    {
        public async Task<HttpResponse> ExecuteAsync(Func<Task<ResponseContext>> action)
        {
            var executionResult = await action.Invoke().ConfigureAwait(false);
            if (typeof(HttpResponse).IsAssignableFrom(executionResult.MethodInvocation.ResultType))
                return executionResult.HttpResponse;
            return executionResult.HttpResponse.EnsureSuccess();
        }
    }
}
