using System;
using NClient.Providers.Transport;
using NClient.Providers.Validation;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UseTransportResponseExtensions
    {
        public static INClientResponseValidationSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportResponseValidationSetter<TRequest, TResponse> transportResponseValidationSetter,
            Predicate<IResponseContext<TRequest, TResponse>> isSuccess,
            Action<IResponseContext<TRequest, TResponse>> onFailure)
        {
            return transportResponseValidationSetter.Use(new ResponseValidatorSettings<TRequest, TResponse>(
                isSuccess, 
                onFailure));
        }
    }
}
