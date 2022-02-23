using NClient.Common.Helpers;
using NClient.Providers.Transport.RestSharp;
using RestSharp;
using RestSharp.Authenticators;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class RestSharpTransportExtensions
    {
        /// <summary>Sets RestSharp based transport.</summary>
        public static INClientSerializationBuilder<TClient, IRestRequest, IRestResponse> UsingRestSharpTransport<TClient>(
            this INClientTransportBuilder<TClient> transportBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(transportBuilder, nameof(transportBuilder));

            return transportBuilder.UsingCustomTransport(
                new RestSharpTransportProvider(),
                new RestSharpTransportRequestBuilderProvider(),
                new RestSharpResponseBuilderProvider());
        }

        /// <summary>Sets RestSharp based transport with authentication.</summary>
        /// <param name="transportBuilder"></param>
        /// <param name="authenticator">The RestSharp authenticator.</param>
        public static INClientSerializationBuilder<TClient, IRestRequest, IRestResponse> UsingRestSharpTransport<TClient>(
            this INClientTransportBuilder<TClient> transportBuilder,
            IAuthenticator authenticator)
            where TClient : class
        {
            Ensure.IsNotNull(transportBuilder, nameof(transportBuilder));

            return transportBuilder.UsingCustomTransport(
                new RestSharpTransportProvider(authenticator),
                new RestSharpTransportRequestBuilderProvider(),
                new RestSharpResponseBuilderProvider());
        }

        /// <summary>Sets RestSharp based transport.</summary>
        public static INClientFactorySerializationBuilder<IRestRequest, IRestResponse> UsingRestSharpTransport(
            this INClientFactoryTransportBuilder transportBuilder)
        {
            Ensure.IsNotNull(transportBuilder, nameof(transportBuilder));

            return transportBuilder.UsingCustomTransport(
                new RestSharpTransportProvider(),
                new RestSharpTransportRequestBuilderProvider(),
                new RestSharpResponseBuilderProvider());
        }

        /// <summary>Sets RestSharp based transport with authentication.</summary>
        /// <param name="transportBuilder"></param>
        /// <param name="authenticator">The RestSharp authenticator.</param>
        public static INClientFactorySerializationBuilder<IRestRequest, IRestResponse> UsingRestSharpTransport(
            this INClientFactoryTransportBuilder transportBuilder,
            IAuthenticator authenticator)
        {
            Ensure.IsNotNull(transportBuilder, nameof(transportBuilder));

            return transportBuilder.UsingCustomTransport(
                new RestSharpTransportProvider(authenticator),
                new RestSharpTransportRequestBuilderProvider(),
                new RestSharpResponseBuilderProvider());
        }
    }
}
