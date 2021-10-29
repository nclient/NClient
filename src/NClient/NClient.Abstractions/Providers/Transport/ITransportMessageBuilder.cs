using System.Threading.Tasks;

namespace NClient.Providers.Transport
{
    // TODO: doc
    public interface ITransportMessageBuilder<TRequest, TResponse>
    {
        Task<TRequest> BuildTransportRequestAsync(IRequest request);
        Task<IResponse> BuildResponseAsync(IRequest request, IResponseContext<TRequest, TResponse> responseContext);
    }
}
