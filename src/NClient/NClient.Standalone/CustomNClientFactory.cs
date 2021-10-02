using NClient.Abstractions;
using NClient.Abstractions.Resilience;
using NClient.Common.Helpers;
using NClient.Customization.Context;

namespace NClient
{
    /// <summary>
    /// The factory used to create the client with custom providers.
    /// </summary>
    public class CustomNClientFactory<TRequest, TResponse> : INClientFactory
    {
        private readonly CustomizerContext<TRequest, TResponse> _customizerContext;
        private readonly IResiliencePolicyProvider<TRequest, TResponse> _defaultResiliencePolicyProvider;
        
        public string Name { get; set; }
        
        public CustomNClientFactory(
            string name,
            CustomizerContext<TRequest, TResponse> customizerContext,
            IResiliencePolicyProvider<TRequest, TResponse> defaultResiliencePolicyProvider)
        {
            Name = name;
            _customizerContext = customizerContext;
            _defaultResiliencePolicyProvider = defaultResiliencePolicyProvider;
        }
        
        public TClient Create<TClient>(string host) where TClient : class
        {
            Ensure.IsNotNull(host, nameof(host));

            return new CustomNClientBuilder<TRequest, TResponse>(_customizerContext, _defaultResiliencePolicyProvider)
                .For<TClient>(host)
                .Build();
        }
    }
}
