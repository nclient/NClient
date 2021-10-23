using System.Threading.Tasks;

namespace NClient.Providers.Transport
{
    // TODO: doc
    public interface ITransportMessageBuilder<TRequest, TResponse>
    {
        Task<TRequest> BuildRequestAsync(IHttpRequest httpRequest);
        Task<IHttpResponse> BuildResponseAsync(IHttpRequest httpRequest, TRequest request, TResponse response);
    }
}
