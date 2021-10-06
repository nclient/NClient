using NClient.Abstractions;
using NClient.Abstractions.Builders;
using NClient.Common.Helpers;
using NClient.Standalone.Builders;

// ReSharper disable once CheckNamespace
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
