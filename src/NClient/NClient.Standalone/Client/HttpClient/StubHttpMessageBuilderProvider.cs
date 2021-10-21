using NClient.Abstractions.Providers.HttpClient;
using NClient.Abstractions.Providers.Serialization;

namespace NClient.Standalone.Client.HttpClient
{
    internal class StubHttpMessageBuilderProvider : IHttpMessageBuilderProvider<IHttpRequest, IHttpResponse>
    {
        public IHttpMessageBuilder<IHttpRequest, IHttpResponse> Create(ISerializer serializer)
        {
            return new StubHttpMessageBuilder();
        }
    }
}
