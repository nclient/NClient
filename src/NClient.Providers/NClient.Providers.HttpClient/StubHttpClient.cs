using System;
using System.Net;
using System.Threading.Tasks;
using NClient.Providers.HttpClient.Abstractions;

namespace NClient.Providers.HttpClient
{
    public class StubHttpClient : IHttpClient
    {
        public Task<HttpResponse> ExecuteAsync(HttpRequest request, Type? bodyType = null)
        {
            var response = new HttpResponse(HttpStatusCode.OK);
            if (bodyType is null)
                return Task.FromResult(response);

            var genericResponse = typeof(HttpResponse<>).MakeGenericType(bodyType);
            return Task.FromResult((HttpResponse)Activator.CreateInstance(genericResponse, response, null));
        }
    }
}
