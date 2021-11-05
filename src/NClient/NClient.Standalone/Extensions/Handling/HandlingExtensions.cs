using System;
using NClient.Common.Helpers;
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
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            Ensure.IsNotNull(beforeRequest, nameof(beforeRequest));
            Ensure.IsNotNull(afterRequest, nameof(afterRequest));
            
            return clientOptionalBuilder.WithAdvancedHandling(x => x
                .ForTransport().Use(new ClientHandlerSettings<TRequest, TResponse>(
                    beforeRequest, afterRequest)));
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithHandling<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            Func<TRequest, TRequest> beforeRequest,
            Func<TResponse, TResponse> afterRequest)
        {
            Ensure.IsNotNull(factoryOptionalBuilder, nameof(factoryOptionalBuilder));
            Ensure.IsNotNull(beforeRequest, nameof(beforeRequest));
            Ensure.IsNotNull(afterRequest, nameof(afterRequest));
            
            return factoryOptionalBuilder.WithAdvancedHandling(x => x
                .ForTransport().Use(new ClientHandlerSettings<TRequest, TResponse>(
                    beforeRequest, afterRequest)));
        }
    }
}
