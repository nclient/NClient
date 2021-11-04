using System.Collections.Generic;
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

        public INClientAdvancedHandlingSetter<TRequest, TResponse> WithCustomTransportHandling(IClientHandlerSettings<TRequest, TResponse> settings, params IClientHandlerSettings<TRequest, TResponse>[] extraSettings)
        {
            return WithCustomTransportHandling(extraSettings.Concat(new[] { settings }));
        }
        
        public INClientAdvancedHandlingSetter<TRequest, TResponse> WithCustomTransportHandling(IEnumerable<IClientHandlerSettings<TRequest, TResponse>> settings)
        {
            return WithCustomTransportHandling(settings
                .Select(x => new ClientHandler<TRequest, TResponse>(x))
                .Cast<IClientHandler<TRequest, TResponse>>()
                .ToArray());
        }
        
        /// <summary>
        /// Sets collection of <see cref="IClientHandler{TRequest,TResponse}"/> used to handle HTTP requests and responses />.
        /// </summary>
        /// <param name="handlers">The collection of handlers.</param>
        public INClientAdvancedHandlingSetter<TRequest, TResponse> WithCustomTransportHandling(IClientHandler<TRequest, TResponse> handler, params IClientHandler<TRequest, TResponse>[] extraHandlers)
        {
            return WithCustomTransportHandling(extraHandlers.Concat(new[] { handler }));
        }
        
        public INClientAdvancedHandlingSetter<TRequest, TResponse> WithCustomTransportHandling(IEnumerable<IClientHandler<TRequest, TResponse>> handlers)
        {
            return WithCustomTransportHandling(handlers
                .Select(x => new ClientHandlerProvider<TRequest, TResponse>(x))
                .Cast<IClientHandlerProvider<TRequest, TResponse>>()
                .ToArray());
        }
        public INClientAdvancedHandlingSetter<TRequest, TResponse> WithCustomTransportHandling(IClientHandlerProvider<TRequest, TResponse> provider, params IClientHandlerProvider<TRequest, TResponse>[] extraProviders)
        {
            return WithCustomTransportHandling(extraProviders.Concat(new[] { provider }));
        }
        
        public INClientAdvancedHandlingSetter<TRequest, TResponse> WithCustomTransportHandling(IEnumerable<IClientHandlerProvider<TRequest, TResponse>> providers)
        {
            var providerCollection = providers as ICollection<IClientHandlerProvider<TRequest, TResponse>> ?? providers.ToArray();
            
            Ensure.AreNotNullItems(providerCollection, nameof(providers));
            
            _builderContextModifier.Add(x => x.WithHandlers(providerCollection));
            return new NClientAdvancedHandlingSetter<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
