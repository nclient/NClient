using System;
using System.Net.Http;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;

namespace NClient.Resilience
{
    internal class DefaultResiliencePolicy : IResiliencePolicy<HttpResponseMessage>
    {
        public async Task<HttpResponseMessage> ExecuteAsync(Func<Task<ResponseContext<HttpResponseMessage>>> action)
        {
            var executionResult = await action.Invoke().ConfigureAwait(false);
            if (executionResult.MethodInvocation.ResultType == typeof(HttpResponseMessage))
                return executionResult.Response;
            if (typeof(HttpResponse).IsAssignableFrom(executionResult.MethodInvocation.ResultType))
                return executionResult.Response;
            return executionResult.Response.EnsureSuccessStatusCode();
        }
    }
}
