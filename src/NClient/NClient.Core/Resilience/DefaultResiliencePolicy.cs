using System;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Invocation;
using NClient.Abstractions.Resilience;

namespace NClient.Core.Resilience
{
    internal class DefaultResiliencePolicy : IResiliencePolicy
    {
        public async Task<HttpResponse> ExecuteAsync(Func<Task<(HttpResponse Response, MethodInvocation MethodInvocation)>> action)
        {
            var executionResult = await action.Invoke().ConfigureAwait(false);
            if (typeof(HttpResponse).IsAssignableFrom(executionResult.MethodInvocation.ResultType))
                return executionResult.Response;
            return executionResult.Response.EnsureSuccess();
        }
    }
}
