using System;
using System.Threading.Tasks;

namespace NClient.Abstractions.HttpClients
{
    public interface IHttpClient
    {
        Task<HttpResponse> ExecuteAsync(HttpRequest request, Type? bodyType = null, Type? errorType = null);
    }
}
