using System.Collections.Generic;
using NClient.Providers.Handling;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientTransportHandlingSetter<TRequest, TResponse>
    {
        INClientHandlingSelector<TRequest, TResponse> Use(IClientHandlerSettings<TRequest, TResponse> settings, params IClientHandlerSettings<TRequest, TResponse>[] extraSettings);
        INClientHandlingSelector<TRequest, TResponse> Use(IEnumerable<IClientHandlerSettings<TRequest, TResponse>> settings);

        INClientHandlingSelector<TRequest, TResponse> Use(IClientHandler<TRequest, TResponse> handler, params IClientHandler<TRequest, TResponse>[] extraHandlers);
        INClientHandlingSelector<TRequest, TResponse> Use(IEnumerable<IClientHandler<TRequest, TResponse>> handlers);

        INClientHandlingSelector<TRequest, TResponse> Use(IClientHandlerProvider<TRequest, TResponse> provider, params IClientHandlerProvider<TRequest, TResponse>[] extraProviders);
        INClientHandlingSelector<TRequest, TResponse> Use(IEnumerable<IClientHandlerProvider<TRequest, TResponse>> providers);
    }
}
