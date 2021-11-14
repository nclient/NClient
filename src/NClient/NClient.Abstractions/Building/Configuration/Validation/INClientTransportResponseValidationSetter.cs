using System.Collections.Generic;
using NClient.Providers.Validation;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientTransportResponseValidationSetter<TRequest, TResponse>
    {
        INClientResponseValidationSelector<TRequest, TResponse> Use(IEnumerable<IResponseValidatorSettings<TRequest, TResponse>> settings);

        INClientResponseValidationSelector<TRequest, TResponse> Use(IEnumerable<IResponseValidator<TRequest, TResponse>> validators);

        INClientResponseValidationSelector<TRequest, TResponse> Use(IEnumerable<IResponseValidatorProvider<TRequest, TResponse>> providers);
    }
}
