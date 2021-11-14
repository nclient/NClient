using System;
using NClient.Providers.Handling;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class TransportHandlingExtensions
    {
        public static INClientHandlingSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportHandlingSetter<TRequest, TResponse> transportHandlingSetter,
            Func<TRequest, TRequest> beforeRequest,
            Func<TResponse, TResponse> afterRequest)
        {
            return transportHandlingSetter.Use(new ClientHandlerSettings<TRequest, TResponse>(
                beforeRequest, 
                afterRequest));
        }
    }
}
