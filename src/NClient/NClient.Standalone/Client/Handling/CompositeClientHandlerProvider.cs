using System.Collections.Generic;
using System.Linq;
using NClient.Providers;
using NClient.Providers.Handling;

namespace NClient.Standalone.Client.Handling
{
    internal class CompositeClientHandlerProvider<TRequest, TResponse> : IClientHandlerProvider<TRequest, TResponse>
    {
        private readonly IReadOnlyCollection<IClientHandlerProvider<TRequest, TResponse>> _clientHandlerProviders;

        public CompositeClientHandlerProvider(IReadOnlyCollection<IClientHandlerProvider<TRequest, TResponse>> clientHandlerProviders)
        {
            _clientHandlerProviders = clientHandlerProviders;
        }
        
        public IClientHandler<TRequest, TResponse> Create(IToolset toolset)
        {
            return new CompositeClientHandler<TRequest, TResponse>(_clientHandlerProviders
                .Select(x => x.Create(toolset))
                .ToArray());
        }
    }
}
