using System;
using NClient.Providers.Transport;
using NClient.Providers.Validation;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UseTransportResponseExtensions
    {
        /// <summary>Sets validation the contents of the response received from transport.</summary>
        /// <param name="transportResponseValidationSetter"></param>
        /// <param name="isSuccess">The predicate for determining the success of the response.</param>
        /// <param name="onFailure">The action that will be invoked if the response is unsuccessful.</param>
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
