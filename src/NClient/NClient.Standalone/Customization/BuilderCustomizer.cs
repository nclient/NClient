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
    internal class BuilderCustomizer<TInterface, TRequest, TResponse> :
        CommonCustomizer<INClientBuilderCustomizer<TInterface, TRequest, TResponse>, TInterface, TRequest, TResponse>,
        INClientBuilderCustomizer<TInterface, TRequest, TResponse>
        where TInterface : class
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
            new ClientValidator(ProxyGenerator).EnsureAsync<TInterface>(_clientInterceptorFactory);
            _clientGenerator = new ClientGenerator(ProxyGenerator);
        }

        public INClientBuilderCustomizer<TInterface, TRequest, TResponse> WithCustomResilience(Action<IResiliencePolicyMethodSelector<TInterface, TRequest, TResponse>> customizer)
        {
            Ensure.IsNotNull(customizer, nameof(customizer));

            customizer(new ResiliencePolicyMethodSelector<TInterface, TRequest, TResponse>(Context));
            return this;
        }

        public override TInterface Build()
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
                Context.Logger as ILogger<TInterface>);

            return _clientGenerator.CreateClient<TInterface>(interceptor);
        }
    }
}
