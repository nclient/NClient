using System;
using NClient.Providers.Handling;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class TransportHandlingExtensions
    {
        public static INClientAdvancedHandlingSetter<TRequest, TResponse> WithTransportHandling<TRequest, TResponse>(
            this INClientAdvancedHandlingSetter<TRequest, TResponse> advancedHandlingSetter,
            Func<TRequest, TRequest> beforeRequest,
            Func<TResponse, TResponse> afterRequest)
        {
            return advancedHandlingSetter.WithCustomTransportHandling(new ClientHandlerSettings<TRequest, TResponse>(
                beforeRequest, 
                afterRequest));
        }
    }
}
