using System.Threading;
using System.Threading.Tasks;

namespace NClient.Providers.Transport
{
    public interface IResponseBuilder<TRequest, TResponse>
    {
        Task<IResponse> BuildAsync(IRequest request, 
            IResponseContext<TRequest, TResponse> responseContext, CancellationToken cancellationToken);
    }
}
