using System.Collections.Generic;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Customization;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Resilience.Providers;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Customization.Context;

namespace NClient.Customization
{
    internal abstract class CommonCustomizer<TSpecificCustomizer, TRequest, TResponse>
        : INClientCommonCustomizer<TSpecificCustomizer, TRequest, TResponse>
        where TSpecificCustomizer : class, INClientCommonCustomizer<TSpecificCustomizer, TRequest, TResponse>
    {
        protected static readonly IProxyGenerator ProxyGenerator = new ProxyGenerator();
        
        protected readonly CustomizerContext<TRequest, TResponse> Context;

        protected CommonCustomizer(CustomizerContext<TRequest, TResponse> context)
        {
            Context = context;
        }

        public TSpecificCustomizer UsingCustomHttpClient(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider, 
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));
            Ensure.IsNotNull(httpMessageBuilderProvider, nameof(httpMessageBuilderProvider));
            Ensure.IsNotNull(httpClientExceptionFactory, nameof(httpClientExceptionFactory));

            Context.SetHttpClientProvider(httpClientProvider, httpMessageBuilderProvider, httpClientExceptionFactory);
            return (this as TSpecificCustomizer)!;
        }
        
        public TSpecificCustomizer UsingCustomSerializer(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));

            Context.SetSerializer(serializerProvider);
            return (this as TSpecificCustomizer)!;
        }

        public TSpecificCustomizer WithCustomHandling(IReadOnlyCollection<IClientHandler<TRequest, TResponse>> handlers)
        {
            Ensure.IsNotNull(handlers, nameof(handlers));

            Context.SetHandlers(handlers);
            return (this as TSpecificCustomizer)!;
        }
        
        public TSpecificCustomizer WithoutHandling()
        {
            Context.ClearHandlers();
            return (this as TSpecificCustomizer)!;
        }

        public TSpecificCustomizer WithForceResilience(IResiliencePolicyProvider<TRequest, TResponse> provider)
        {
            Ensure.IsNotNull(provider, nameof(provider));
            
            Context.SetResiliencePolicy(new MethodResiliencePolicyProviderAdapter<TRequest, TResponse>(provider));
            return (this as TSpecificCustomizer)!;
        }
        
        public TSpecificCustomizer WithIdempotentResilience(
            IResiliencePolicyProvider<TRequest, TResponse> idempotentMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
        {
            Ensure.IsNotNull(idempotentMethodProvider, nameof(idempotentMethodProvider));
            
            Context.SetResiliencePolicy(new IdempotentMethodResiliencePolicyProvider<TRequest, TResponse>(idempotentMethodProvider, otherMethodProvider));
            return (this as TSpecificCustomizer)!;
        }

        public TSpecificCustomizer WithSafeResilience(
            IResiliencePolicyProvider<TRequest, TResponse> safeMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
        {
            Ensure.IsNotNull(safeMethodProvider, nameof(safeMethodProvider));
            
            Context.SetResiliencePolicy(new SafeMethodResiliencePolicyProvider<TRequest, TResponse>(safeMethodProvider, otherMethodProvider));
            return (this as TSpecificCustomizer)!;
        }

        public TSpecificCustomizer WithCustomResilience(IMethodResiliencePolicyProvider<TRequest, TResponse> methodResiliencePolicyProvider)
        {
            Ensure.IsNotNull(methodResiliencePolicyProvider, nameof(methodResiliencePolicyProvider));

            Context.SetResiliencePolicy(methodResiliencePolicyProvider);
            return (this as TSpecificCustomizer)!;
        }
        public TSpecificCustomizer WithoutResilience()
        {
            Context.ClearResiliencePolicy();
            return (this as TSpecificCustomizer)!;
        }

        public TSpecificCustomizer WithLogging(ILoggerFactory loggerFactory)
        {
            Ensure.IsNotNull(loggerFactory, nameof(loggerFactory));

            Context.SetLogging(loggerFactory);
            return (this as TSpecificCustomizer)!;
        }
        
        public TSpecificCustomizer WithLogging(ILogger logger)
        {
            Context.SetLogging(logger);
            return (this as TSpecificCustomizer)!;
        }
        public TSpecificCustomizer WithoutLogging()
        {
            Context.ClearLogging();
            return (this as TSpecificCustomizer)!;
        }
    }
}
