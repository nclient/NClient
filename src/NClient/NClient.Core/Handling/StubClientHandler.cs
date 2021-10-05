using System.Threading.Tasks;
using NClient.Abstractions.Handling;
using NClient.Abstractions.Invocation;

namespace NClient.Core.Handling
{
    internal class StubClientHandler<TRequest, TResponse> : IClientHandler<TRequest, TResponse>
    {
        public Task<TRequest> HandleRequestAsync(TRequest request, MethodInvocation methodInvocation)
        {
            return Task.FromResult(request);
        }

        public Task<TResponse> HandleResponseAsync(TResponse response, MethodInvocation methodInvocation)
        {
            return Task.FromResult(response);
        }
    }
}
