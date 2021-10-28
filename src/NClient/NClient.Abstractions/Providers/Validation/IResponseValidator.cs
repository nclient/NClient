using System.Threading.Tasks;
using NClient.Providers.Resilience;

namespace NClient.Providers.Validation
{
    public interface IResponseValidator<TRequest, TResponse>
    {
        bool IsSuccess(IResponseContext<TRequest, TResponse> responseContext);
        Task OnFailureAsync(IResponseContext<TRequest, TResponse> responseContext);
    }
}
