using System.Collections.Generic;
using NClient.Providers.Validation;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientTransportResponseValidationSetter<TRequest, TResponse>
    {
        INClientResponseValidationSelector<TRequest, TResponse> Use(IResponseValidatorSettings<TRequest, TResponse> settings, params IResponseValidatorSettings<TRequest, TResponse>[] extraSettings);
        INClientResponseValidationSelector<TRequest, TResponse> Use(IEnumerable<IResponseValidatorSettings<TRequest, TResponse>> settings);

        INClientResponseValidationSelector<TRequest, TResponse> Use(IResponseValidator<TRequest, TResponse> validator, params IResponseValidator<TRequest, TResponse>[] extraValidators);
        INClientResponseValidationSelector<TRequest, TResponse> Use(IEnumerable<IResponseValidator<TRequest, TResponse>> validators);

        INClientResponseValidationSelector<TRequest, TResponse> Use(IResponseValidatorProvider<TRequest, TResponse> provider, params IResponseValidatorProvider<TRequest, TResponse>[] extraProviders);
        INClientResponseValidationSelector<TRequest, TResponse> Use(IEnumerable<IResponseValidatorProvider<TRequest, TResponse>> providers);
    }
}
