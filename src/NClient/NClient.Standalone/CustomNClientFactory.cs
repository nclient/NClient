using NClient.Common.Helpers;
using NClient.Standalone.ClientProxy.Building;
using NClient.Standalone.ClientProxy.Building.Context;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>
    /// The factory used to create the client with custom providers.
    /// </summary>
    internal class CustomNClientFactory<TRequest, TResponse> : INClientFactory
    {
        private readonly BuilderContext<TRequest, TResponse> _builderContext;
        
        public string Name { get; set; }
        
        public CustomNClientFactory(string name, BuilderContext<TRequest, TResponse> builderContext)
        {
            Name = name;
            _builderContext = builderContext;
        }
        
        public TClient Create<TClient>(string host) where TClient : class
        {
            Ensure.IsNotNull(host, nameof(host));
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_builderContext.WithHost(host)).Build();
        }
    }
}
