using System.Net.Http;
using System.Threading;
using NClient.Common.Helpers;
using NClient.Providers.Transport.SystemNetHttp.Helpers;

namespace NClient.Providers.Transport.SystemNetHttp
{
    /// <summary>The System.Net.Http based provider for a component that can create <see cref="ITransport{TRequest,TResponse}"/> instances.</summary>
    public class SystemNetHttpTransportProvider : ITransportProvider<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string? _httpClientName;

        /// <summary>Initializes the System.Net.Http based HTTP client provider.</summary>
        public SystemNetHttpTransportProvider()
        {
            _httpClientFactory = new SystemNetHttpClientFactory(new HttpClient
            {
                Timeout = Timeout.InfiniteTimeSpan
            });
        }

        /// <summary>Initializes the System.Net.Http based HTTP client provider.</summary>
        /// <param name="httpClient">The system <see cref="HttpClient"/>.</param>
        public SystemNetHttpTransportProvider(HttpClient httpClient)
        {
            Ensure.IsNotNull(httpClient, nameof(httpClient));

            _httpClientFactory = new SystemNetHttpClientFactory(httpClient);
        }

        /// <summary>Initializes the System.Net.Http based HTTP client provider.</summary>
        /// <param name="httpClientFactory">The factory abstraction used to create instance of <see cref="HttpClient"/> instances.</param>
        /// <param name="httpClientName">The logical name of <see cref="HttpClient"/> to create.</param>
        public SystemNetHttpTransportProvider(IHttpClientFactory httpClientFactory, string? httpClientName = null)
        {
            Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));

            _httpClientFactory = httpClientFactory;
            _httpClientName = httpClientName;
        }

        /// <summary>Creates System.Net.Http based transport.</summary>
        /// <param name="toolset">Tools that help implement providers.</param>
        public ITransport<HttpRequestMessage, HttpResponseMessage> Create(IToolset toolset)
        {
            Ensure.IsNotNull(toolset, nameof(toolset));

            return new SystemNetHttpTransport(_httpClientFactory.CreateClient(_httpClientName));
        }
    }
}
