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
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            Predicate<IResponseContext<TRequest, TResponse>> isSuccess,
            Action<IResponseContext<TRequest, TResponse>> onFailure)
            where TClient : class
        {
            return optionalBuilder.WithAdvancedResponseValidation(x => x
                .ForTransport().Use(new ResponseValidatorSettings<TRequest, TResponse>(
                    isSuccess, onFailure)));
        }

        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithResponseValidation<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            Predicate<IResponseContext<TRequest, TResponse>> isSuccess,
            Action<IResponseContext<TRequest, TResponse>> onFailure)
        {
            return optionalBuilder.WithAdvancedResponseValidation(x => x
                .ForTransport().Use(new ResponseValidatorSettings<TRequest, TResponse>(
                    isSuccess, onFailure)));
        }
    }
}
