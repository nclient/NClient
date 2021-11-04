using System.Linq;
using NClient.Providers.Handling;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ExtraHandlingExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithHandling<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            IClientHandler<TRequest, TResponse> handler, params IClientHandler<TRequest, TResponse>[] extraHandlers)
            where TClient : class
        {
            return clientOptionalBuilder.WithHandling(extraHandlers.Concat(new[] { handler }));
        }

        public static INClientHandlingSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportHandlingSetter<TRequest, TResponse> transportHandlingSetter,
            IClientHandlerSettings<TRequest, TResponse> settings, params IClientHandlerSettings<TRequest, TResponse>[] extraSettings)
        {
            return transportHandlingSetter.Use(extraSettings.Concat(new[] { settings }));
        }

        public static INClientHandlingSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportHandlingSetter<TRequest, TResponse> transportHandlingSetter,
            IClientHandler<TRequest, TResponse> handler, params IClientHandler<TRequest, TResponse>[] extraHandlers)
        {
            return transportHandlingSetter.Use(extraHandlers.Concat(new[] { handler }));
        }

        public static INClientHandlingSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportHandlingSetter<TRequest, TResponse> transportHandlingSetter,
            IClientHandlerProvider<TRequest, TResponse> provider, params IClientHandlerProvider<TRequest, TResponse>[] extraProviders)
        {
            return transportHandlingSetter.Use(extraProviders.Concat(new[] { provider }));
        }
    }
}
