// ReSharper disable once CheckNamespace

namespace NClient.Providers.Validation
{
    public interface IResponseValidatorProvider<TRequest, TResponse>
    {
        IResponseValidator<TRequest, TResponse> Create(IToolSet toolset);
    }
}
