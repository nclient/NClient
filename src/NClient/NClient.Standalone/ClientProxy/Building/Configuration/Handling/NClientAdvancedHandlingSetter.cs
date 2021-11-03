using System.Linq;
using NClient.Common.Helpers;
using NClient.Providers.Handling;
using NClient.Standalone.Client.Handling;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Handling
{
    internal class NClientAdvancedHandlingSetter<TRequest, TResponse> : INClientAdvancedHandlingSetter<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        
        public NClientAdvancedHandlingSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier)
        {
            _builderContextModifier = builderContextModifier;
        }
        
        public INClientAdvancedHandlingSetter<TRequest, TResponse> WithCustomTransportHandling(params IClientHandlerSettings<TRequest, TResponse>[] clientHandlerSettings)
        {
            return WithCustomTransportHandling(clientHandlerSettings
                .Select(x => new ClientHandler<TRequest, TResponse>(x))
                .Cast<IClientHandler<TRequest, TResponse>>()
                .ToArray());
        }

        /// <summary>
        /// Sets collection of <see cref="IClientHandler{TRequest,TResponse}"/> used to handle HTTP requests and responses />.
        /// </summary>
        /// <param name="handlers">The collection of handlers.</param>
        public INClientAdvancedHandlingSetter<TRequest, TResponse> WithCustomTransportHandling(params IClientHandler<TRequest, TResponse>[] handlers)
        {
            return WithCustomTransportHandling(handlers
                .Select(x => new ClientHandlerProvider<TRequest, TResponse>(x))
                .Cast<IClientHandlerProvider<TRequest, TResponse>>()
                .ToArray());
        }

        public INClientAdvancedHandlingSetter<TRequest, TResponse> WithCustomTransportHandling(params IClientHandlerProvider<TRequest, TResponse>[] providers)
        {
            Ensure.IsNotNull(providers, nameof(providers));
            
            _builderContextModifier.Add(x => x.WithHandlers(providers));
            return new NClientAdvancedHandlingSetter<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
