using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NClient.Abstractions.Handling;

namespace NClient.Standalone.Client.Handling
{
    internal class ClientHandlerDecorator<TRequest, TResponse> : IClientHandler<TRequest, TResponse>
    {
        private readonly IReadOnlyCollection<IClientHandler<TRequest, TResponse>> _clientHandlers;

        public ClientHandlerDecorator(IReadOnlyCollection<IClientHandler<TRequest, TResponse>> clientHandlers)
        {
            _clientHandlers = clientHandlers
                .OrderByDescending(x => x is IOrderedClientHandler)
                .ThenBy(x => (x as IOrderedClientHandler)?.Order)
                .ToArray();
        }

        public async Task<TRequest> HandleRequestAsync(TRequest request)
        {
            var handledHttpRequest = request;
            foreach (var clientHandler in _clientHandlers)
            {
                handledHttpRequest = await clientHandler
                    .HandleRequestAsync(handledHttpRequest)
                    .ConfigureAwait(false);
            }

            return handledHttpRequest;
        }

        public async Task<TResponse> HandleResponseAsync(TResponse response)
        {
            var handledHttpResponse = response;
            foreach (var clientHandler in _clientHandlers)
            {
                handledHttpResponse = await clientHandler
                    .HandleResponseAsync(handledHttpResponse)
                    .ConfigureAwait(false);
            }

            return handledHttpResponse;
        }
    }
}
