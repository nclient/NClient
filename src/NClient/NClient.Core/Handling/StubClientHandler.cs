using System.Threading.Tasks;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Invocation;

namespace NClient.Core.Handling
{
    internal class StubClientHandler : IClientHandler
    {
        public Task<HttpRequest> HandleRequestAsync(
            HttpRequest httpRequest, MethodInvocation methodInvocation)
        {
            return Task.FromResult(httpRequest);
        }

        public Task<HttpResponse> HandleResponseAsync(
            HttpResponse httpResponse, MethodInvocation methodInvocation)
        {
            return Task.FromResult(httpResponse);
        }
    }
}
