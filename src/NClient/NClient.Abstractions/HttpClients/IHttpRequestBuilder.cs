using System.Net.Http;
using System.Threading.Tasks;
using NClient.Providers.Results.HttpMessages;

namespace NClient.Abstractions.HttpClients
{
    // TODO: doc
    public interface IHttpMessageBuilder<TRequest, TResponse>
    {
        Task<TRequest> BuildRequestAsync(IHttpRequest httpRequest);
        Task<IHttpResponse> BuildResponseAsync(IHttpRequest httpRequest, TResponse response);
    }
}
