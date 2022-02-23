using NClient.Common.Helpers;
using NClient.Standalone.ClientProxy.Building;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>The builder used to create the client with custom providers.</summary>
    internal class NClientBuilder : INClientBuilder
    {
        /// <summary>Sets client interface type and web service host.</summary>
        /// <param name="host">The address of the web service host.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public INClientApiBuilder<TClient> For<TClient>(string host) where TClient : class
        {
            Ensure.IsNotNull(host, nameof(host));
            return new NClientApiBuilder<TClient>(host);
        }
    }
}
