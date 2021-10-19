using System.Linq;
using NClient.Abstractions.Building;
using NClient.Abstractions.Handling;
using NClient.Standalone.Client.Handling;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ClientHandlerExtensions
    {
        /// <summary>
        /// Sets collection of <see cref="IClientHandler{TRequest,TResponse}"/> used to handle HTTP requests and responses />.
        /// </summary>
        /// <param name="handlers">The collection of handlers.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomHandling<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            params IClientHandler<TRequest, TResponse>[] handlers)
            where TClient : class
        {
            return clientOptionalBuilder.WithCustomHandling(handlers
                .Select(x => new ClientHandlerProvider<TRequest, TResponse>(x))
                .Cast<IClientHandlerProvider<TRequest, TResponse>>()
                .ToArray());
        }
        
        /// <summary>
        /// Sets collection of <see cref="IClientHandler{TRequest,TResponse}"/> used to handle HTTP requests and responses />.
        /// </summary>
        /// <param name="handlers">The collection of handlers.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomHandling<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            params IClientHandler<TRequest, TResponse>[] handlers)
        {
            return factoryOptionalBuilder.WithCustomHandling(handlers
                .Select(x => new ClientHandlerProvider<TRequest, TResponse>(x))
                .Cast<IClientHandlerProvider<TRequest, TResponse>>()
                .ToArray());
        }
    }
}
