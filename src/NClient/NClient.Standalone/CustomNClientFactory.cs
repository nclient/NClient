using NClient.Abstractions;
using NClient.Common.Helpers;
using NClient.Customization;
using NClient.Customization.Context;

namespace NClient
{
    /// <summary>
    /// The factory used to create the client with custom providers.
    /// </summary>
    public class CustomNClientFactory<TRequest, TResponse> : INClientFactory
    {
        private readonly CustomizerContext<TRequest, TResponse> _customizerContext;
        
        public string Name { get; set; }
        
        public CustomNClientFactory(string name, CustomizerContext<TRequest, TResponse> customizerContext)
        {
            Name = name;
            _customizerContext = customizerContext;
        }
        
        public TClient Create<TClient>(string host) where TClient : class
        {
            Ensure.IsNotNull(host, nameof(host));

            return new ClientOptionalBuilder<TClient, TRequest, TResponse>(_customizerContext).Build();
        }
    }
}
