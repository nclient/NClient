using NClient.Providers.Validation;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientAdvancedResponseValidationSetter<TRequest, TResponse>
    {
        INClientAdvancedResponseValidationSetter<TRequest, TResponse> WithCustomResponseValidation(params IResponseValidatorSettings<TRequest, TResponse>[] responseValidatorSettings);

        INClientAdvancedResponseValidationSetter<TRequest, TResponse> WithCustomResponseValidation(params IResponseValidator<TRequest, TResponse>[] responseValidators);

        INClientAdvancedResponseValidationSetter<TRequest, TResponse> WithCustomResponseValidation(params IResponseValidatorProvider<TRequest, TResponse>[] responseValidatorProvider);
    }
}
