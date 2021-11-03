using System;
using NClient.Providers.Handling;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ClientHandlerExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithCustomHandling<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            Func<TRequest, TRequest> beforeRequest,
            Func<TResponse, TResponse> afterRequest)
            where TClient : class
        {
            return clientOptionalBuilder.AsAdvanced()
                .WithCustomHandling(new ClientHandlerSettings<TRequest, TResponse>(
                    beforeRequest, 
                    afterRequest));
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithCustomHandling<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            Func<TRequest, TRequest> beforeRequest,
            Func<TResponse, TResponse> afterRequest)
        {
            return factoryOptionalBuilder.WithCustomHandling(new ClientHandlerSettings<TRequest, TResponse>(
                beforeRequest, 
                afterRequest));
        }
    }
}
