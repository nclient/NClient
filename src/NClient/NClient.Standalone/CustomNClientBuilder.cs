using NClient.Abstractions;
using NClient.Abstractions.Builders;
using NClient.Builders;
using NClient.Common.Helpers;

namespace NClient
{
    /// <summary>
    /// The builder used to create the client with custom providers.
    /// </summary>
    public class CustomNClientBuilder : INClientBuilder
    {
        public INClientHttpClientBuilder<TClient> For<TClient>(string host) where TClient : class
        {
            Ensure.IsNotNull(host, nameof(host));
            return new NClientHttpClientBuilder<TClient>(host);
        }
    }
}
