using System.Net.Http;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;

namespace NClient.Providers.HttpClient.System
{
    public class SystemHttpMessageBuilderProvider : IHttpMessageBuilderProvider<HttpRequestMessage>
    {
        public IHttpMessageBuilder<HttpRequestMessage> Create(ISerializer serializer)
        {
            return new SystemHttpMessageBuilder(serializer);
        }
    }
}
