using System;
using NClient.Common.Helpers;
using NClient.Providers.Handling;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class HandlingExtensions
    {
        /// <summary>Sets handling operations that handles the transport messages.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="beforeRequest">These function will be executed before a request.</param>
        /// <param name="afterRequest">These function will be executed after a request.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithHandling<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            Func<TRequest, TRequest> beforeRequest,
            Func<TResponse, TResponse> afterRequest) 
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(beforeRequest, nameof(beforeRequest));
            Ensure.IsNotNull(afterRequest, nameof(afterRequest));
            
            return optionalBuilder.WithAdvancedHandling(x => x
                .ForTransport().Use(new ClientHandlerSettings<TRequest, TResponse>(
                    beforeRequest, afterRequest)));
        }
        
        /// <summary>Sets handling operations that handles the transport messages.</summary>
        /// <param name="factoryOptionalBuilder"></param>
        /// <param name="beforeRequest">These function will be executed before a request.</param>
        /// <param name="afterRequest">These function will be executed after a request.</param>
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
