using System.Threading.Tasks;

namespace NClient.Abstractions.HttpClients
{
    public interface IHttpMessageBuilder<TRequest, TResponse>
    {
        Task<TRequest> BuildRequestAsync(HttpRequest httpRequest);
        Task<HttpResponse> BuildResponseAsync(HttpRequest httpRequest, TResponse response);
    }
}
