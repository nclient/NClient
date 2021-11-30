﻿using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Handling;

namespace NClient.Standalone.Client.Handling
{
    internal class ClientHandler<TRequest, TResponse> : IClientHandler<TRequest, TResponse>
    {
        private readonly IClientHandlerSettings<TRequest, TResponse> _clientHandlerSettings;

        public ClientHandler(IClientHandlerSettings<TRequest, TResponse> clientHandlerSettings)
        {
            _clientHandlerSettings = clientHandlerSettings;
        }

        public Task<TRequest> HandleRequestAsync(TRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_clientHandlerSettings.BeforeRequest(request));
        }

        public Task<TResponse> HandleResponseAsync(TResponse response, CancellationToken cancellationToken)
        {
            return Task.FromResult(_clientHandlerSettings.AfterResponse(response));
        }
    }
}
