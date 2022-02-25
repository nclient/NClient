using System;
using NClient.Providers.Handling;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class TransportHandlingExtensions
    {
        /// <summary>Sets handling operations that handles the transport messages.</summary>
        /// <param name="transportHandlingSetter"></param>
        /// <param name="beforeRequest">These function will be executed before a request.</param>
        /// <param name="afterRequest">These function will be executed after a request.</param>
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
