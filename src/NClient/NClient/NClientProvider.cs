using System.Net.Http;
using NClient.Abstractions.Customization;
using NClient.Common.Helpers;

namespace NClient
{
    /// <summary>
    /// The static provider used to create the client.
    /// </summary>
    public static class NClientProvider
    {
        /// <summary>
        /// Sets the main client settings.
        /// </summary>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <typeparam name="TClient">The type of interface of controller used to create the client.</typeparam>
        public static INClientBuilderCustomizer<TClient, HttpRequestMessage, HttpResponseMessage> Use<TClient>(string host)
            where TClient : class
        {
            Ensure.IsNotNull(host, nameof(host));

            return new NClientBuilder().For<TClient>(host);
        }
    }
}
