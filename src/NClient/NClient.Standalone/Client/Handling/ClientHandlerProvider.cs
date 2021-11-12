using NClient.Providers;
using NClient.Providers.Handling;

namespace NClient.Standalone.Client.Handling
{
    internal class ClientHandlerProvider<TRequest, TResponse> : IClientHandlerProvider<TRequest, TResponse>
    {
        private readonly IClientHandler<TRequest, TResponse> _clientHandler;
        
        public ClientHandlerProvider(IClientHandler<TRequest, TResponse> clientHandler)
        {
            _clientHandler = clientHandler;
        }
        
        public IClientHandler<TRequest, TResponse> Create(IToolset toolset)
        {
            return _clientHandler;
        }
    }
}
