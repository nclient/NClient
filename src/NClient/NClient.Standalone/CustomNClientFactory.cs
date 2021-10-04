using NClient.Abstractions;
using NClient.Builders;
using NClient.Builders.Context;
using NClient.Common.Helpers;

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
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_builderContext).Build();
        }
    }
}
