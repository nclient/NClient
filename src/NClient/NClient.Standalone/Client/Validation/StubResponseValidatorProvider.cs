using NClient.Abstractions.Providers.Validation;

namespace NClient.Standalone.Client.Validation
{
    public class StubResponseValidatorProvider<TRequest, TResponse> : IResponseValidatorProvider<TRequest, TResponse>
    {
        public IResponseValidator<TRequest, TResponse> Create()
        {
            return new StubResponseValidator<TRequest, TResponse>();
        }
    }
}
