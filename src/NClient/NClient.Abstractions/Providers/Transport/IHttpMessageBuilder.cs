using System.Threading.Tasks;

namespace NClient.Abstractions.Providers.Transport
{
    // TODO: doc
    public interface IHttpMessageBuilder<TRequest, TResponse>
    {
        Task<TRequest> BuildRequestAsync(IHttpRequest httpRequest);
        Task<IHttpResponse> BuildResponseAsync(IHttpRequest httpRequest, TRequest request, TResponse response);
    }
}
