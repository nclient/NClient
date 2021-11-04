using System;
using NClient.Providers.Handling;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class HandlingExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithHandling<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            Func<TRequest, TRequest> beforeRequest,
            Func<TResponse, TResponse> afterRequest) 
            where TClient : class
        {
            return clientOptionalBuilder.AsAdvanced()
                .WithHandling(x => x
                    .WithCustomTransportHandling(new ClientHandlerSettings<TRequest, TResponse>(
                        beforeRequest, 
                        afterRequest)))
                .AsBasic();
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithHandling<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            Func<TRequest, TRequest> beforeRequest,
            Func<TResponse, TResponse> afterRequest)
        {
            return factoryOptionalBuilder
                .WithHandling(x => x
                    .WithCustomTransportHandling(new ClientHandlerSettings<TRequest, TResponse>(
                        beforeRequest, 
                        afterRequest)));
        }
    }
}
