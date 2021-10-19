using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;

namespace NClient.Standalone.Client.Handling
{
    internal class ClientHandlerProviderDecorator<TClient, TRequest, TResponse> : IClientHandlerProvider<TRequest, TResponse>
    {
        private readonly IReadOnlyCollection<IClientHandlerProvider<TRequest, TResponse>> _clientHandlerProviders;
        private readonly ILogger<TClient>? _logger;
        
        public ClientHandlerProviderDecorator(
            IReadOnlyCollection<IClientHandlerProvider<TRequest, TResponse>> clientHandlerProviders,
            ILogger<TClient>? logger)
        {
            _clientHandlerProviders = clientHandlerProviders;
            _logger = logger;
        }
        
        public IClientHandler<TRequest, TResponse> Create()
        {
            return new ClientHandlerDecorator<TClient, TRequest, TResponse>(
                _clientHandlerProviders.Select(x => x.Create()).ToArray(),
                _logger);
        }
    }
}
