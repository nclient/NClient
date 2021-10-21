namespace NClient.Abstractions.Providers.Validation
{
    public interface IResponseValidatorProvider<TRequest, TResponse>
    {
        IResponseValidator<TRequest, TResponse> Create();
    }
}
