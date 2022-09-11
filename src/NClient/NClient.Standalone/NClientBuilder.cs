using System;
using NClient.Common.Helpers;
using NClient.Providers.Host;
using NClient.Standalone.Client.Host;
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
            return For<TClient>(new Host(host));
        }
        public INClientApiBuilder<TClient> For<TClient>(IHost? host) where TClient : class
        {
            return new NClientApiBuilder<TClient>(host);
        }
    }
}
