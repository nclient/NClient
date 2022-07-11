using System.Collections.Generic;
using NClient.Providers.Validation;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>Setter for custom functionality to validating transport responses.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface INClientTransportResponseValidationSetter<TRequest, TResponse>
    {
        /// <summary>Sets the response validator settings for validation the contents of the response received from transport.</summary>
        /// <param name="settings">The response validator settings for validation the contents of the response received from transport.</param>
        INClientResponseValidationSelector<TRequest, TResponse> Use(IEnumerable<IResponseValidatorSettings<TRequest, TResponse>> settings);

        /// <summary>Sets the response validator for validation the contents of the response received from transport.</summary>
        /// <param name="validators">The response validator for validation the contents of the response received from transport.</param>
        INClientResponseValidationSelector<TRequest, TResponse> Use(IEnumerable<IResponseValidator<TRequest, TResponse>> validators);

        /// <summary>Sets the provider of response validator for validation the contents of the response received from transport.</summary>
        /// <param name="providers">The provider of response validator for validation the contents of the response received from transport.</param>
        INClientResponseValidationSelector<TRequest, TResponse> Use(IEnumerable<IResponseValidatorProvider<TRequest, TResponse>> providers);
    }
}
