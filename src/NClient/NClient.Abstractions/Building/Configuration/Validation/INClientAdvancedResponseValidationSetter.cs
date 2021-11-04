using System.Collections.Generic;
using NClient.Providers.Validation;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientAdvancedResponseValidationSetter<TRequest, TResponse>
    {
        INClientAdvancedResponseValidationSetter<TRequest, TResponse> WithCustomResponseValidation(IResponseValidatorSettings<TRequest, TResponse> settings, params IResponseValidatorSettings<TRequest, TResponse>[] extraSettings);
        INClientAdvancedResponseValidationSetter<TRequest, TResponse> WithCustomResponseValidation(IEnumerable<IResponseValidatorSettings<TRequest, TResponse>> settings);

        INClientAdvancedResponseValidationSetter<TRequest, TResponse> WithCustomResponseValidation(IResponseValidator<TRequest, TResponse> validator, params IResponseValidator<TRequest, TResponse>[] extraValidators);
        INClientAdvancedResponseValidationSetter<TRequest, TResponse> WithCustomResponseValidation(IEnumerable<IResponseValidator<TRequest, TResponse>> validators);

        INClientAdvancedResponseValidationSetter<TRequest, TResponse> WithCustomResponseValidation(IResponseValidatorProvider<TRequest, TResponse> provider, params IResponseValidatorProvider<TRequest, TResponse>[] extraProviders);
        INClientAdvancedResponseValidationSetter<TRequest, TResponse> WithCustomResponseValidation(IEnumerable<IResponseValidatorProvider<TRequest, TResponse>> providers);
    }
}
