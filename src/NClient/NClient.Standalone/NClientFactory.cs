using System;
using NClient.Common.Helpers;
using NClient.Providers.Host;
using NClient.Standalone.Client.Host;
using NClient.Standalone.ClientProxy.Building;
using NClient.Standalone.ClientProxy.Building.Context;

// ReSharper disable once CheckNamespace
namespace NClient
{
    internal class NClientFactory<TRequest, TResponse> : INClientFactory
    {
        private readonly BuilderContext<TRequest, TResponse> _builderContext;
        
        public string Name { get; set; }
        
        public NClientFactory(string name, BuilderContext<TRequest, TResponse> builderContext)
        {
            Name = name;
            _builderContext = builderContext;
        }
        
        public TClient Create<TClient>(string host) where TClient : class
        {
            Ensure.IsNotNull(host, nameof(host));
            return Create<TClient>(new Uri(host));
        }
        
        public TClient Create<TClient>(Uri host) where TClient : class
        {
            Ensure.IsNotNull(host, nameof(host));
            return Create<TClient>(new Host(host));
        }
        
        public TClient Create<TClient>(IHost host) where TClient : class
        {
            Ensure.IsNotNull(host, nameof(host));
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_builderContext.WithHost(host)).Build();
        }
    }
}
