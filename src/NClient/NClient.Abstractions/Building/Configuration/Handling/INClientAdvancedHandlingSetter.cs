using System.Collections.Generic;
using NClient.Providers.Handling;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientAdvancedHandlingSetter<TRequest, TResponse>
    {
        INClientAdvancedHandlingSetter<TRequest, TResponse> WithCustomTransportHandling(IClientHandlerSettings<TRequest, TResponse> settings, params IClientHandlerSettings<TRequest, TResponse>[] extraSettings);
        INClientAdvancedHandlingSetter<TRequest, TResponse> WithCustomTransportHandling(IEnumerable<IClientHandlerSettings<TRequest, TResponse>> settings);

        INClientAdvancedHandlingSetter<TRequest, TResponse> WithCustomTransportHandling(IClientHandler<TRequest, TResponse> handler, params IClientHandler<TRequest, TResponse>[] extraHandlers);
        INClientAdvancedHandlingSetter<TRequest, TResponse> WithCustomTransportHandling(IEnumerable<IClientHandler<TRequest, TResponse>> handlers);

        INClientAdvancedHandlingSetter<TRequest, TResponse> WithCustomTransportHandling(IClientHandlerProvider<TRequest, TResponse> provider, params IClientHandlerProvider<TRequest, TResponse>[] extraProviders);
        INClientAdvancedHandlingSetter<TRequest, TResponse> WithCustomTransportHandling(IEnumerable<IClientHandlerProvider<TRequest, TResponse>> providers);
    }
}
