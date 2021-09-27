using System.Threading.Tasks;

namespace NClient.Abstractions.HttpClients
{
    public interface IHttpMessageBuilder<TRequest, TResponse>
    {
        Task<TRequest> BuildAsync(HttpRequest request);
        Task<HttpResponse> BuildAsync(HttpRequest request, TResponse customResponse);
    }
}
