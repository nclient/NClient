using System;
using NClient.Providers.Handling;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class HandlingExtensions
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
        
        public static INClientHandlingSetter<TRequest, TResponse> WithTransportHandling<TRequest, TResponse>(
            this INClientHandlingSetter<TRequest, TResponse> handlingSetter,
            Func<TRequest, TRequest> beforeRequest,
            Func<TResponse, TResponse> afterRequest)
        {
            return handlingSetter.AsAdvanced()
                .WithCustomTransportHandling(new ClientHandlerSettings<TRequest, TResponse>(
                    beforeRequest, 
                    afterRequest));
        }
    }
}
