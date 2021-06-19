using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Invocation;

namespace NClient.Core.Handling
{
    internal class ClientHandlerDecorator<TClient> : IClientHandler
    {
        private readonly IReadOnlyCollection<IClientHandler> _clientHandlers;
        private readonly ILogger<TClient>? _logger;

        public ClientHandlerDecorator(
            IReadOnlyCollection<IClientHandler> clientHandlers, 
            ILogger<TClient>? logger)
        {
            _clientHandlers = clientHandlers;
            _logger = logger;
        }
        
        public async Task<HttpRequest> HandleRequestAsync(
            HttpRequest httpRequest, MethodInvocation methodInvocation)
        {
            var handledHttpRequest = httpRequest;
            foreach (var clientHandler in _clientHandlers)
            {
                handledHttpRequest = await clientHandler
                    .HandleRequestAsync(handledHttpRequest, methodInvocation)
                    .ConfigureAwait(false);
            }

            return handledHttpRequest;
        }

        public async Task<HttpResponse> HandleResponseAsync(
            HttpResponse httpResponse, MethodInvocation methodInvocation)
        {
            var handledHttpResponse = httpResponse;
            foreach (var clientHandler in _clientHandlers)
            {
                handledHttpResponse = await clientHandler
                    .HandleResponseAsync(handledHttpResponse, methodInvocation)
                    .ConfigureAwait(false);
            }

            return handledHttpResponse;
        }
    }
}