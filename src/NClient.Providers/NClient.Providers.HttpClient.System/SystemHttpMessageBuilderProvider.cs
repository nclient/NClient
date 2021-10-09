using System.Net.Http;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;

namespace NClient.Providers.HttpClient.System
{
    public class SystemHttpMessageBuilderProvider : IHttpMessageBuilderProvider<HttpRequestMessage, HttpResponseMessage>
    {
        public IHttpMessageBuilder<HttpRequestMessage, HttpResponseMessage> Create(ISerializer serializer)
        {
            return new SystemHttpMessageBuilder(serializer);
        }
    }
}
