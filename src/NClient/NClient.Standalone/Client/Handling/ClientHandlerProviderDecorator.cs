using System.Collections.Generic;
using System.Linq;
using NClient.Providers.Handling;

namespace NClient.Standalone.Client.Handling
{
    internal class ClientHandlerProviderDecorator<TRequest, TResponse> : IClientHandlerProvider<TRequest, TResponse>
    {
        private readonly IReadOnlyCollection<IClientHandlerProvider<TRequest, TResponse>> _clientHandlerProviders;

        public ClientHandlerProviderDecorator(IReadOnlyCollection<IClientHandlerProvider<TRequest, TResponse>> clientHandlerProviders)
        {
            _clientHandlerProviders = clientHandlerProviders;
        }
        
        public IClientHandler<TRequest, TResponse> Create()
        {
            return new ClientHandlerDecorator<TRequest, TResponse>(_clientHandlerProviders
                .Select(x => x.Create())
                .ToArray());
        }
    }
}
