// ReSharper disable once CheckNamespace

namespace NClient.Providers.Validation
{
    /// <summary>The provider of response validator for validation the contents of the response received from transport.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface IResponseValidatorProvider<TRequest, TResponse>
    {
        /// <summary>Creates the response validator for validation the contents of the response received from transport.</summary>
        /// <param name="toolset">Tools that help implement providers.</param>
        IResponseValidator<TRequest, TResponse> Create(IToolset toolset);
    }
}
