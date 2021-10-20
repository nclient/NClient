using System.Threading.Tasks;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Validation;

namespace NClient.Standalone.Client.Validation
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
