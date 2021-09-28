using System.Threading.Tasks;

namespace NClient.Abstractions.HttpClients
{
    public interface IHttpMessageBuilder<TRequest, TResponse>
    {
        Task<TRequest> BuildRequestAsync(HttpRequest request);
        Task<HttpResponse> BuildResponseAsync(HttpRequest request, TResponse customResponse);
    }
}
