using System.Threading.Tasks;
using NClient.Abstractions.Providers.Resilience;
using NClient.Abstractions.Providers.Validation;

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
