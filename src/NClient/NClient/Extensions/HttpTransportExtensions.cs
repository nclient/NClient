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
        /// <param name="clientAdvancedTransportBuilder"></param>
        public static INClientAdvancedSerializerBuilder<TClient, HttpRequestMessage, HttpResponseMessage> UsingHttpTransport<TClient>(
            this INClientAdvancedTransportBuilder<TClient> clientAdvancedTransportBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedTransportBuilder, nameof(clientAdvancedTransportBuilder));

            return clientAdvancedTransportBuilder.UsingSystemHttpTransport();
        }
        
        /// <summary>
        /// Sets System.Net.Http based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="clientTransportBuilder"></param>
        public static INClientSerializerBuilder<TClient, HttpRequestMessage, HttpResponseMessage> UsingHttpTransport<TClient>(
            this INClientTransportBuilder<TClient> clientTransportBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientTransportBuilder, nameof(clientTransportBuilder));

            return clientTransportBuilder.UsingSystemHttpTransport();
        }
        
        /// <summary>
        /// Sets System.Net.Http based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="clientAdvancedTransportBuilder"></param>
        public static INClientFactoryAdvancedSerializerBuilder<HttpRequestMessage, HttpResponseMessage> UsingHttpTransport(
            this INClientFactoryAdvancedTransportBuilder clientAdvancedTransportBuilder)
        {
            Ensure.IsNotNull(clientAdvancedTransportBuilder, nameof(clientAdvancedTransportBuilder));

            return clientAdvancedTransportBuilder.UsingSystemHttpTransport();
        }
        
        /// <summary>
        /// Sets System.Net.Http based <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instance of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="clientTransportBuilder"></param>
        public static INClientFactorySerializerBuilder<HttpRequestMessage, HttpResponseMessage> UsingHttpTransport(
            this INClientFactoryTransportBuilder clientTransportBuilder)
        {
            Ensure.IsNotNull(clientTransportBuilder, nameof(clientTransportBuilder));

            return clientTransportBuilder.UsingSystemHttpTransport();
        }
    }
}
