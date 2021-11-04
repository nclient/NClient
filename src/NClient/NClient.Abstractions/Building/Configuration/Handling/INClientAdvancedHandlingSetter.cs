using NClient.Providers.Handling;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientAdvancedHandlingSetter<TRequest, TResponse>
    {
        INClientAdvancedHandlingSetter<TRequest, TResponse> WithCustomTransportHandling(params IClientHandlerSettings<TRequest, TResponse>[] handlerSettings);

        INClientAdvancedHandlingSetter<TRequest, TResponse> WithCustomTransportHandling(params IClientHandler<TRequest, TResponse>[] handlers);

        INClientAdvancedHandlingSetter<TRequest, TResponse> WithCustomTransportHandling(params IClientHandlerProvider<TRequest, TResponse>[] providers);
    }
}
