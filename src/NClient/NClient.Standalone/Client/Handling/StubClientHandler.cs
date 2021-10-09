using System.Threading.Tasks;
using NClient.Abstractions.Handling;

namespace NClient.Standalone.Client.Handling
{
    internal class StubClientHandler<TRequest, TResponse> : IClientHandler<TRequest, TResponse>
    {
        public Task<TRequest> HandleRequestAsync(TRequest request)
        {
            return Task.FromResult(request);
        }

        public Task<TResponse> HandleResponseAsync(TResponse response)
        {
            return Task.FromResult(response);
        }
    }
}
