using System.Linq;
using NClient.Providers.Validation;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ExtraResponseValidationExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithResponseValidation<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            IResponseValidator<TRequest, TResponse> validator, params IResponseValidator<TRequest, TResponse>[] extraValidators)
            where TClient : class
        {
            return optionalBuilder.WithResponseValidation(extraValidators.Concat(new[] { validator }));
        }

        public static INClientResponseValidationSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportResponseValidationSetter<TRequest, TResponse> transportResponseValidationSetter,
            IResponseValidatorSettings<TRequest, TResponse> settings, params IResponseValidatorSettings<TRequest, TResponse>[] extraSettings)
        {
            return transportResponseValidationSetter.Use(extraSettings.Concat(new[] { settings }));
        }

        public static INClientResponseValidationSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportResponseValidationSetter<TRequest, TResponse> transportResponseValidationSetter,
            IResponseValidator<TRequest, TResponse> validator, params IResponseValidator<TRequest, TResponse>[] extraValidators)
        {
            return transportResponseValidationSetter.Use(extraValidators.Concat(new[] { validator }));
        }

        public static INClientResponseValidationSelector<TRequest, TResponse> Use<TRequest, TResponse>(
            this INClientTransportResponseValidationSetter<TRequest, TResponse> transportResponseValidationSetter,
            IResponseValidatorProvider<TRequest, TResponse> provider, params IResponseValidatorProvider<TRequest, TResponse>[] extraProviders)
        {
            return transportResponseValidationSetter.Use(extraProviders.Concat(new[] { provider }));
        }
    }
}
