using System;
using System.Threading.Tasks;

namespace NClient.Providers.HttpClient.Abstractions
{
    public interface IHttpClient
    {
        Task<HttpResponse> ExecuteAsync(HttpRequest request, Type? bodyType = null);
    }
}
