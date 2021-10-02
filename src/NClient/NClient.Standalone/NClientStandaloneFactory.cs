using NClient.Abstractions;
using NClient.Abstractions.Resilience;
using NClient.Common.Helpers;
using NClient.Customization.Context;

namespace NClient
{
    /// <summary>
    /// The factory used to create the client with custom providers.
    /// </summary>
    public class NClientStandaloneFactory<TRequest, TResponse> : INClientFactory
    {
        private readonly CustomizerContext<TRequest, TResponse> _customizerContext;
        private readonly IResiliencePolicyProvider<TRequest, TResponse> _defaultResiliencePolicyProvider;
        
        public NClientStandaloneFactory(
            CustomizerContext<TRequest, TResponse> customizerContext,
            IResiliencePolicyProvider<TRequest, TResponse> defaultResiliencePolicyProvider)
        {
            _customizerContext = customizerContext;
            _defaultResiliencePolicyProvider = defaultResiliencePolicyProvider;
        }

        public TClient Create<TClient>(string host) where TClient : class
        {
            Ensure.IsNotNull(host, nameof(host));

            return new NClientStandaloneBuilder<TRequest, TResponse>(_customizerContext, _defaultResiliencePolicyProvider)
                .For<TClient>(host)
                .Build();
        }
    }
}
