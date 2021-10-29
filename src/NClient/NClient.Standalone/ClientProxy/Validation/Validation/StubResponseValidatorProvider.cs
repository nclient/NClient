using NClient.Providers.Validation;

namespace NClient.Standalone.ClientProxy.Validation.Validation
{
    public class StubResponseValidatorProvider<TRequest, TResponse> : IResponseValidatorProvider<TRequest, TResponse>
    {
        public IResponseValidator<TRequest, TResponse> Create()
        {
            return new StubResponseValidator<TRequest, TResponse>();
        }
    }
}
