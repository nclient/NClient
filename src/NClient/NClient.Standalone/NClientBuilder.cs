using System;
using NClient.Common.Helpers;
using NClient.Standalone.ClientProxy.Building;

// ReSharper disable once CheckNamespace
namespace NClient
{
    internal class NClientBuilder : INClientBuilder
    {
        public INClientApiBuilder<TClient> For<TClient>(string host) where TClient : class
        {
            Ensure.IsNotNull(host, nameof(host));
            return For<TClient>(new Uri(host));
        }
        
        public INClientApiBuilder<TClient> For<TClient>(Uri host) where TClient : class
        {
            Ensure.IsNotNull(host, nameof(host));
            return new NClientApiBuilder<TClient>(host);
        }
    }
}
