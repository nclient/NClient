using System.Threading.Tasks;
using NClient.Abstractions.Providers.Resilience;

namespace NClient.Abstractions.Providers.Validation
{
    public interface IResponseValidator<TRequest, TResponse>
    {
        bool IsSuccess(IResponseContext<TRequest, TResponse> responseContext);
        Task OnFailureAsync(IResponseContext<TRequest, TResponse> responseContext);
    }
}
