using System;
using NClient.Providers.Transport;
using NClient.Providers.Validation;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ResponseValidationExtensions
    {
        /// <summary>Sets validation the contents of the response received from transport.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="isSuccess">The predicate for determining the success of the response.</param>
        /// <param name="onFailure">The action that will be invoked if the response is unsuccessful.</param>
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

        /// <summary>Sets validation the contents of the response received from transport.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="isSuccess">The predicate for determining the success of the response.</param>
        /// <param name="onFailure">The action that will be invoked if the response is unsuccessful.</param>
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
