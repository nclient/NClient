using System.Threading.Tasks;
using NClient.Providers.Resilience;
using NClient.Providers.Validation;

namespace NClient.Standalone.ClientProxy.Validation.Validation
{
    public class StubResponseValidator<TRequest, TResponse> : IResponseValidator<TRequest, TResponse>
    {
        public bool IsSuccess(IResponseContext<TRequest, TResponse> responseContext)
        {
            return true;
        }

        public Task OnFailureAsync(IResponseContext<TRequest, TResponse> responseContext)
        {
            return Task.CompletedTask;
        }
    }
}
