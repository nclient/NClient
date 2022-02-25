using System.Net.Http;

namespace NClient.Providers.Transport.SystemNetHttp
{
    /// <summary>The provider that can create builder for transforming a System.Net.Http response to NClient response.</summary>
    public class SystemNetHttpResponseBuilderProvider : IResponseBuilderProvider<HttpRequestMessage, HttpResponseMessage>
    {
        /// <summary>Creates builder for transforming a System.Net.Http response to NClient response.</summary>
        /// <param name="toolset">Tools that help implement providers.</param>
        public IResponseBuilder<HttpRequestMessage, HttpResponseMessage> Create(IToolset toolset)
        {
            return new SystemNetHttpResponseBuilder();
        }
    }
}
