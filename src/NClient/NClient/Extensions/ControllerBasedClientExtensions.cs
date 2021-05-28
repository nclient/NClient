using System;
using System.Net.Http;
using System.Text.Json;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Common.Helpers;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Serialization.System;
using Polly;

namespace NClient.Extensions
{
    public static class ControllerBasedClientExtensions
    {
        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        public static IControllerBasedClientBuilder<TInterface, TController> WithCustomHttpClient<TInterface, TController>(
            this IControllerBasedClientBuilder<TInterface, TController> clientBuilder, 
            IHttpClientFactory httpClientFactory, string? httpClientName = null)
            where TInterface : class
            where TController : TInterface
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));
            Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));
            
            return clientBuilder.WithCustomHttpClient(new SystemHttpClientProvider(httpClientFactory, httpClientName));
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        public static IControllerBasedClientBuilder<TInterface, TController> WithCustomHttpClient<TInterface, TController>(
            this IControllerBasedClientBuilder<TInterface, TController> clientBuilder, 
            HttpMessageHandler httpMessageHandler)
            where TInterface : class
            where TController : TInterface
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));
            Ensure.IsNotNull(httpMessageHandler, nameof(httpMessageHandler));
            
            return clientBuilder.WithCustomHttpClient(new SystemHttpClientProvider(httpMessageHandler));
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use WithResiliencePolicy<T> method.")]
        public static IControllerBasedClientBuilder<TInterface, TController> WithCustomSerializer<TInterface, TController>(
            this IControllerBasedClientBuilder<TInterface, TController> clientBuilder, 
            JsonSerializerOptions jsonSerializerOptions)
            where TInterface : class
            where TController : TInterface
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));
            
            return clientBuilder.WithCustomSerializer(new SystemSerializerProvider(jsonSerializerOptions));
        }

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use WithResiliencePolicy<T> method.")]
        public static IControllerBasedClientBuilder<TInterface, TController> WithResiliencePolicy<TInterface, TController>(
            this IControllerBasedClientBuilder<TInterface, TController> clientBuilder, 
            IAsyncPolicy<HttpResponse> asyncPolicy)
            where TInterface : class
            where TController : TInterface
        {
            Ensure.IsNotNull(clientBuilder, nameof(clientBuilder));
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));
            
            return clientBuilder.WithResiliencePolicy(new PollyResiliencePolicyProvider(asyncPolicy));
        }
    }
}
