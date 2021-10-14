using NClient.Abstractions;
using NClient.Abstractions.Building;
using NClient.Common.Helpers;
using NClient.Standalone.ClientProxy.Building;

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
