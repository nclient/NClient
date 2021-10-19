using System;
using NClient.Abstractions.Building;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient
{
    // TODO: doc
    public static class EnsuringExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> EnsuringCustomSuccess<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            Predicate<IResponseContext<TRequest, TResponse>> isSuccess,
            Action<IResponseContext<TRequest, TResponse>> onFailure)
            where TClient : class
        {
            return clientOptionalBuilder.EnsuringCustomSuccess(new EnsuringSettings<TRequest, TResponse>(
                isSuccess, 
                onFailure));
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> EnsuringCustomSuccess<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            Predicate<IResponseContext<TRequest, TResponse>> isSuccess,
            Action<IResponseContext<TRequest, TResponse>> onFailure)
        {
            return factoryOptionalBuilder.EnsuringCustomSuccess(new EnsuringSettings<TRequest, TResponse>(
                isSuccess, 
                onFailure));
        }
    }
}
