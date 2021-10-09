using System.Threading.Tasks;
using NClient.Abstractions.Handling;
using NClient.Abstractions.Invocation;

namespace NClient.Standalone.Client.Handling
{
    internal class StubClientHandler<TRequest, TResponse> : IClientHandler<TRequest, TResponse>
    {
        public Task<TRequest> HandleRequestAsync(TRequest request, IMethodInvocation methodInvocation)
        {
            return Task.FromResult(request);
        }

        public Task<TResponse> HandleResponseAsync(TResponse response, IMethodInvocation methodInvocation)
        {
            return Task.FromResult(response);
        }
    }
}
