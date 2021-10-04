using System.Net.Http;
using NClient.Abstractions;

namespace NClient
{
    public class NClientGallery : INClientGallery
    {
        public INClientPreConfiguredBuilder<HttpRequestMessage, HttpResponseMessage> BasicClient { get; set; }
        public INClientPreConfiguredBuilder<HttpRequestMessage, HttpResponseMessage> StandardClient { get; set; }
    }
}
