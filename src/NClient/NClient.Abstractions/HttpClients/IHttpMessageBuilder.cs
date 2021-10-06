using System.Threading.Tasks;

namespace NClient.Abstractions.HttpClients
{
    public interface IHttpMessageBuilder<TRequest, TResponse>
    {
        Task<TRequest> BuildRequestAsync(IHttpRequest httpRequest);
        Task<IHttpResponse> BuildResponseAsync(IHttpRequest httpRequest, TResponse response);
    }
}
