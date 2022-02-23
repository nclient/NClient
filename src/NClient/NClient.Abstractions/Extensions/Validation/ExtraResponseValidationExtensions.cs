using System.Linq;
using NClient.Providers.Validation;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ExtraResponseValidationExtensions
    {
        /// <summary>Sets response validation of the contents received from transport.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="validator">The response validator for validation the contents of the response received from transport.</param>
        /// <param name="extraValidators">The additional validators that will also be set.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithResponseValidation<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            IResponseValidator<TRequest, TResponse> validator, params IResponseValidator<TRequest, TResponse>[] extraValidators)
            where TClient : class
        {
            return optionalBuilder.WithResponseValidation(extraValidators.Concat(new[] { validator }));
        }

        /// <summary>Sets setting for response validation of the contents received from transport.</summary>
        /// <param name="transportResponseValidationSetter"></param>
        /// <param name="settings">The response validator settings for validation the contents of the response received from transport.</param>
        /// <param name="extraSettings">The additional settings that will also be set.</param>
        public static INClientResponseValidationSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportResponseValidationSetter<TRequest, TResponse> transportResponseValidationSetter,
            IResponseValidatorSettings<TRequest, TResponse> settings, params IResponseValidatorSettings<TRequest, TResponse>[] extraSettings)
        {
            return transportResponseValidationSetter.Use(extraSettings.Concat(new[] { settings }));
        }

        /// <summary>Sets response validation of the contents received from transport.</summary>
        /// <param name="transportResponseValidationSetter"></param>
        /// <param name="validator">The response validator for validation the contents of the response received from transport.</param>
        /// <param name="extraValidators">The additional validators that will also be set.</param>
        public static INClientResponseValidationSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportResponseValidationSetter<TRequest, TResponse> transportResponseValidationSetter,
            IResponseValidator<TRequest, TResponse> validator, params IResponseValidator<TRequest, TResponse>[] extraValidators)
        {
            return transportResponseValidationSetter.Use(extraValidators.Concat(new[] { validator }));
        }

        /// <summary>Sets providers for response validation of the contents received from transport.</summary>
        /// <param name="transportResponseValidationSetter"></param>
        /// <param name="provider">The provider of response validator for validation the contents of the response received from transport.</param>
        /// <param name="extraProviders">The additional validator providers that will also be set.</param>
        public static INClientResponseValidationSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportResponseValidationSetter<TRequest, TResponse> transportResponseValidationSetter,
            IResponseValidatorProvider<TRequest, TResponse> provider, params IResponseValidatorProvider<TRequest, TResponse>[] extraProviders)
        {
            return transportResponseValidationSetter.Use(extraProviders.Concat(new[] { provider }));
        }
    }
}
