using System.Net.Http;
using NClient.Common.Helpers;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class HttpTransportExtensions
    {
        /// <summary>
        /// Sets System.Net.Http based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="transportBuilder"></param>
        public static INClientSerializationBuilder<TClient, HttpRequestMessage, HttpResponseMessage> UsingHttpTransport<TClient>(
            this INClientTransportBuilder<TClient> transportBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(transportBuilder, nameof(transportBuilder));

            return transportBuilder.UsingSystemNetHttpTransport();
        }

        /// <summary>
        /// Sets System.Net.Http based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="transportBuilder"></param>
        public static INClientFactorySerializationBuilder<HttpRequestMessage, HttpResponseMessage> UsingHttpTransport(
            this INClientFactoryTransportBuilder transportBuilder)
        {
            Ensure.IsNotNull(transportBuilder, nameof(transportBuilder));

            return transportBuilder.UsingSystemNetHttpTransport();
        }
    }
}
