using System.Net.Http;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Exceptions;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.HttpClient.System
{
    public class DefaultSystemEnsuringSettings : EnsuringSettings<HttpRequestMessage, HttpResponseMessage>
    {
        public DefaultSystemEnsuringSettings() : base(
            isSuccess: x => x.Response.IsSuccessStatusCode,
            onFailure: x =>
            {
                try
                {
                    x.Response.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException e)
                {
                    throw new HttpClientException<HttpRequestMessage, HttpResponseMessage>(x.Request, x.Response, e.Message, e);
                }
            })
        {
        }
    }
}
