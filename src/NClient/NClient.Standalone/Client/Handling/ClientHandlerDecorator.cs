using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;

namespace NClient.Standalone.Client.Handling
{
    internal class ClientHandlerDecorator<TClient, TRequest, TResponse> : IClientHandler<TRequest, TResponse>
    {
        private readonly IReadOnlyCollection<IClientHandler<TRequest, TResponse>> _clientHandlers;
        private readonly ILogger<TClient>? _logger;

        public ClientHandlerDecorator(
            IReadOnlyCollection<IClientHandler<TRequest, TResponse>> clientHandlers,
            ILogger<TClient>? logger)
        {
            _clientHandlers = clientHandlers;
            _logger = logger;
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
