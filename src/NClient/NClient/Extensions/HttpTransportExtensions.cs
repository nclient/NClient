using System.Net.Http;
using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class HttpTransportExtensions
    {
        /// <summary>Sets default HTTP transport based on System.Net.Http. This is an alias for the <see cref="UsingSystemNetHttpTransport"/> method.</summary>
        public static INClientSerializationBuilder<TClient, HttpRequestMessage, HttpResponseMessage> UsingHttpTransport<TClient>(
            this INClientTransportBuilder<TClient> transportBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(transportBuilder, nameof(transportBuilder));

            return transportBuilder.UsingSystemNetHttpTransport();
        }

        /// <summary>Sets default HTTP transport based on System.Net.Http. This is an alias for the <see cref="UsingSystemNetHttpTransport"/> method.</summary>
        public static INClientFactorySerializationBuilder<HttpRequestMessage, HttpResponseMessage> UsingHttpTransport(
            this INClientFactoryTransportBuilder transportBuilder)
        {
            Ensure.IsNotNull(transportBuilder, nameof(transportBuilder));

            return transportBuilder.UsingSystemNetHttpTransport();
        }
    }
}
