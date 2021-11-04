using System;
using NClient.Providers.Transport;
using NClient.Providers.Validation;

// ReSharper disable once CheckNamespace
namespace NClient
{
    // TODO: doc
    public static class ResponseValidationExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithResponseValidation<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            Predicate<IResponseContext<TRequest, TResponse>> isSuccess,
            Action<IResponseContext<TRequest, TResponse>> onFailure)
            where TClient : class
        {
            return clientOptionalBuilder.AsAdvanced()
                .WithResponseValidation(x => x
                    .WithCustomResponseValidation(new ResponseValidatorSettings<TRequest, TResponse>(
                        isSuccess, 
                        onFailure)))
                .AsBasic();
        }
        
        public static INClientAdvancedResponseValidationSetter<TRequest, TResponse> WithCustomResponseValidation<TRequest, TResponse>(
            this INClientAdvancedResponseValidationSetter<TRequest, TResponse> advancedResponseValidationSetter,
            Predicate<IResponseContext<TRequest, TResponse>> isSuccess,
            Action<IResponseContext<TRequest, TResponse>> onFailure)
        {
            return advancedResponseValidationSetter.WithCustomResponseValidation(new ResponseValidatorSettings<TRequest, TResponse>(
                isSuccess, 
                onFailure));
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithResponseValidation<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            Predicate<IResponseContext<TRequest, TResponse>> isSuccess,
            Action<IResponseContext<TRequest, TResponse>> onFailure)
        {
            return factoryOptionalBuilder.WithCustomResponseValidation(new ResponseValidatorSettings<TRequest, TResponse>(
                isSuccess, 
                onFailure));
        }
    }
}
