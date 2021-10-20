using System.Threading.Tasks;
using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Validation
{
    public interface IResponseValidator<TRequest, TResponse>
    {
        bool IsSuccess(IResponseContext<TRequest, TResponse> responseContext);
        Task OnFailureAsync(IResponseContext<TRequest, TResponse> responseContext);
    }
}
