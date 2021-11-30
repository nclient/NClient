using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Handling;

namespace NClient.Standalone.ClientProxy.Validation.Handling
{
    internal class StubClientHandler<TRequest, TResponse> : IClientHandler<TRequest, TResponse>
    {
        public Task<TRequest> HandleRequestAsync(TRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(request);
        }

        public Task<TResponse> HandleResponseAsync(TResponse response, CancellationToken cancellationToken)
        {
            return Task.FromResult(response);
        }
    }
}
