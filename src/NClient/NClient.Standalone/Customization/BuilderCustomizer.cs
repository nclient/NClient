using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Customization;
using NClient.Abstractions.Customization.Resilience;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Resilience.Providers;
using NClient.ClientGeneration;
using NClient.Common.Helpers;
using NClient.Core.Interceptors;
using NClient.Core.Validation;
using NClient.Customization.Context;
using NClient.Customization.Resilience;

namespace NClient.Customization
{
    internal class BuilderCustomizer<TClient, TRequest, TResponse> :
        CommonCustomizer<INClientBuilderCustomizer<TClient, TRequest, TResponse>, TClient, TRequest, TResponse>,
        INClientBuilderCustomizer<TClient, TRequest, TResponse>
        where TClient : class
    {
        private readonly IResiliencePolicyProvider<TRequest, TResponse> _defaultResiliencePolicyProvider;
        private readonly IClientInterceptorFactory _clientInterceptorFactory;
        private readonly IClientGenerator _clientGenerator;

        public BuilderCustomizer(
            CustomizerContext<TRequest, TResponse> context,
            IResiliencePolicyProvider<TRequest, TResponse> defaultResiliencePolicyProvider) 
            : base(context)
        {
            _defaultResiliencePolicyProvider = defaultResiliencePolicyProvider;
            _clientInterceptorFactory = new ClientInterceptorFactory(ProxyGenerator);
            new ClientValidator(ProxyGenerator).EnsureAsync<TClient>(_clientInterceptorFactory);
            _clientGenerator = new ClientGenerator(ProxyGenerator);
        }

        public INClientBuilderCustomizer<TClient, TRequest, TResponse> WithCustomResilience(Action<IResiliencePolicyMethodSelector<TClient, TRequest, TResponse>> customizer)
        {
            Ensure.IsNotNull(customizer, nameof(customizer));

            customizer(new ResiliencePolicyMethodSelector<TClient, TRequest, TResponse>(Context));
            return this;
        }

        public override TClient Build()
        {
            var interceptor = _clientInterceptorFactory.Create(
                new Uri(Context.Host),
                Context.HttpClientProvider,
                Context.HttpMessageBuilderProvider,
                Context.HttpClientExceptionFactory,
                Context.SerializerProvider,
                Context.ClientHandlers.ToArray(),
                Context.MethodResiliencePolicyProvider
                ?? new MethodResiliencePolicyProviderAdapter<TRequest, TResponse>(
                    Context.AllMethodsResiliencePolicyProvider ?? _defaultResiliencePolicyProvider, 
                    Context.MethodsWithResiliencePolicy),
                Context.Logger as ILogger<TClient>);

            return _clientGenerator.CreateClient<TClient>(interceptor);
        }
    }
}
