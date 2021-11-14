using System.Collections.Generic;
using System.Linq;
using NClient.Common.Helpers;
using NClient.Providers.Handling;
using NClient.Standalone.Client.Handling;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Handling
{
    internal class NClientTransportHandlingSetter<TRequest, TResponse> : INClientTransportHandlingSetter<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        
        public NClientTransportHandlingSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier)
        {
            _builderContextModifier = builderContextModifier;
        }

        public INClientHandlingSelector<TRequest, TResponse> Use(IClientHandlerSettings<TRequest, TResponse> settings, params IClientHandlerSettings<TRequest, TResponse>[] extraSettings)
        {
            return Use(extraSettings.Concat(new[] { settings }));
        }
        
        public INClientHandlingSelector<TRequest, TResponse> Use(IEnumerable<IClientHandlerSettings<TRequest, TResponse>> settings)
        {
            return Use(settings
                .Select(x => new ClientHandler<TRequest, TResponse>(x))
                .Cast<IClientHandler<TRequest, TResponse>>()
                .ToArray());
        }
        
        /// <summary>
        /// Sets collection of <see cref="IClientHandler{TRequest,TResponse}"/> used to handle HTTP requests and responses />.
        /// </summary>
        /// <param name="handlers">The collection of handlers.</param>
        public INClientHandlingSelector<TRequest, TResponse> Use(IClientHandler<TRequest, TResponse> handler, params IClientHandler<TRequest, TResponse>[] extraHandlers)
        {
            return Use(extraHandlers.Concat(new[] { handler }));
        }
        
        public INClientHandlingSelector<TRequest, TResponse> Use(IEnumerable<IClientHandler<TRequest, TResponse>> handlers)
        {
            var handlerCollection = handlers as ICollection<IClientHandler<TRequest, TResponse>> ?? handlers.ToArray();

            Ensure.AreNotNullItems(handlerCollection, nameof(handlers));
            
            return Use(handlerCollection
                .Select(x => new ClientHandlerProvider<TRequest, TResponse>(x))
                .Cast<IClientHandlerProvider<TRequest, TResponse>>()
                .ToArray());
        }
        public INClientHandlingSelector<TRequest, TResponse> Use(IClientHandlerProvider<TRequest, TResponse> provider, params IClientHandlerProvider<TRequest, TResponse>[] extraProviders)
        {
            return Use(extraProviders.Concat(new[] { provider }));
        }
        
        public INClientHandlingSelector<TRequest, TResponse> Use(IEnumerable<IClientHandlerProvider<TRequest, TResponse>> providers)
        {
            var providerCollection = providers as ICollection<IClientHandlerProvider<TRequest, TResponse>> ?? providers.ToArray();
            
            Ensure.AreNotNullItems(providerCollection, nameof(providers));
            
            _builderContextModifier.Add(x => x.WithHandlers(providerCollection));
            return new NClientHandlingSelector<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
