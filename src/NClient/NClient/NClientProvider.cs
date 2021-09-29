using System.Net.Http;
using NClient.Abstractions;
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
        /// <typeparam name="TInterface">The type of interface of controller used to create the client.</typeparam>
        public static INClientBuilderCustomizer<TInterface, HttpRequestMessage, HttpResponseMessage> Use<TInterface>(string host)
            where TInterface : class
        {
            Ensure.IsNotNull(host, nameof(host));

            return new NClientBuilder().Use<TInterface>(host);
        }
    }
}
