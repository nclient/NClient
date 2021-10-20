namespace NClient.Abstractions.Validation
{
    public interface IResponseValidatorProvider<TRequest, TResponse>
    {
        IResponseValidator<TRequest, TResponse> Create();
    }
}
