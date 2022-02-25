using System.Linq;
using NClient.Providers.Handling;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ExtraHandlingExtensions
    {
        /// <summary>Sets handling operations that handles the transport messages.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="handler">The handler that provides custom functionality to handling transport requests and responses.</param>
        /// <param name="extraHandlers">The additional handlers that will also be set.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithHandling<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            IClientHandler<TRequest, TResponse> handler, params IClientHandler<TRequest, TResponse>[] extraHandlers)
            where TClient : class
        {
            return optionalBuilder.WithHandling(extraHandlers.Concat(new[] { handler }));
        }

        /// <summary>Sets settings for handling operations that handles the transport messages.</summary>
        /// <param name="transportHandlingSetter"></param>
        /// <param name="settings">Settings for handling operations that handles the transport messages.</param>
        /// <param name="extraSettings">The additional handler settings that will also be set.</param>
        public static INClientHandlingSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportHandlingSetter<TRequest, TResponse> transportHandlingSetter,
            IClientHandlerSettings<TRequest, TResponse> settings, params IClientHandlerSettings<TRequest, TResponse>[] extraSettings)
        {
            return transportHandlingSetter.Use(extraSettings.Concat(new[] { settings }));
        }

        /// <summary>Sets handling operations that handles the transport messages.</summary>
        /// <param name="transportHandlingSetter"></param>
        /// <param name="handler">The handler that provides custom functionality to handling transport requests and responses.</param>
        /// <param name="extraHandlers">The additional handlers that will also be set.</param>
        public static INClientHandlingSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportHandlingSetter<TRequest, TResponse> transportHandlingSetter,
            IClientHandler<TRequest, TResponse> handler, params IClientHandler<TRequest, TResponse>[] extraHandlers)
        {
            return transportHandlingSetter.Use(extraHandlers.Concat(new[] { handler }));
        }

        /// <summary>Sets provider of handling operations that handles the transport messages.</summary>
        /// <param name="transportHandlingSetter"></param>
        /// <param name="provider">The providers creating handlers that provide custom functionality to handling transport requests and responses.</param>
        /// <param name="extraProviders">The additional handler providers that will also be set.</param>
        public static INClientHandlingSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportHandlingSetter<TRequest, TResponse> transportHandlingSetter,
            IClientHandlerProvider<TRequest, TResponse> provider, params IClientHandlerProvider<TRequest, TResponse>[] extraProviders)
        {
            return transportHandlingSetter.Use(extraProviders.Concat(new[] { provider }));
        }
    }
}
