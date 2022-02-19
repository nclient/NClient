using System;
using NClient.Common.Helpers;
using NClient.Standalone.ClientProxy.Building;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>
    /// The builder used to create the client with custom providers.
    /// </summary>
    public class NClientBuilder : INClientBuilder
    {
        public INClientApiBuilder<TClient> For<TClient>(Uri baseUri) where TClient : class
        {
            Ensure.IsNotNull(baseUri, nameof(baseUri));
            return new NClientApiBuilder<TClient>(baseUri);
        }
    }
}
