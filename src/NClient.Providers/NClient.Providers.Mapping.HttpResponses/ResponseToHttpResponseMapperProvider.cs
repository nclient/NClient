using System.Net.Http;

namespace NClient.Providers.Mapping.HttpResponses
{
    /// <summary>The provider of the mapper that converts System.Net.Http transport messages into HTTP NClient responses with deserialized data.</summary>
    public class ResponseToHttpResponseMapperProvider : IResponseMapperProvider<HttpRequestMessage, HttpResponseMessage>
    {
        /// <summary>Creates the mapper that converts System.Net.Http transport messages into HTTP NClient responses with deserialized data.</summary>
        /// <param name="toolset">Tools that help implement providers.</param>
        public IResponseMapper<HttpRequestMessage, HttpResponseMessage> Create(IToolset toolset)
        {
            return new ResponseToHttpResponseMapper(toolset);
        }
    }
}
