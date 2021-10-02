using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions.Customization;
using NClient.Common.Helpers;
using NClient.Core.Helpers;
using NClient.Extensions.DependencyInjection.Extensions;

namespace NClient.Extensions.DependencyInjection
{
    public static class AddNClientFactoryExtensions
    {
        private static readonly IGuidProvider GuidProvider = new GuidProvider();
        
        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="factoryName">The name of the factory.</param>
        public static IHttpClientBuilder AddNClientFactory(this IServiceCollection serviceCollection,
            string? factoryName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));

            factoryName ??= GuidProvider.Create().ToString();
            var httpClientName = GuidProvider.Create().ToString();
            
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var factoryCustomizer = CreatePreConfiguredCustomizer(serviceProvider, factoryName, httpClientName);
                return factoryCustomizer.Build();
            }).AddHttpClient(httpClientName);
        }

        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configure">The action to configure NClient settings.</param>
        /// <param name="factoryName">The name of the factory.</param>
        public static IHttpClientBuilder AddNClientFactory(this IServiceCollection serviceCollection,
            Func<INClientFactoryCustomizer<HttpRequestMessage, HttpResponseMessage>, INClientFactoryCustomizer<HttpRequestMessage, HttpResponseMessage>> configure,
            string? factoryName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(configure, nameof(configure));

            factoryName ??= GuidProvider.Create().ToString();
            var httpClientName = GuidProvider.Create().ToString();
            
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var factoryCustomizer = CreatePreConfiguredCustomizer(serviceProvider, factoryName, httpClientName);
                return configure(factoryCustomizer).Build();
            }).AddHttpClient(httpClientName);
        }

        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configure">The action to configure NClient settings.</param>
        /// <param name="factoryName">The name of the factory.</param>
        public static IHttpClientBuilder AddNClientFactory(this IServiceCollection serviceCollection,
            Func<IServiceProvider, INClientFactoryCustomizer<HttpRequestMessage, HttpResponseMessage>, INClientFactoryCustomizer<HttpRequestMessage, HttpResponseMessage>> configure,
            string? factoryName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(configure, nameof(configure));

            factoryName ??= GuidProvider.Create().ToString();
            var httpClientName = GuidProvider.Create().ToString();

            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var factoryCustomizer = CreatePreConfiguredCustomizer(serviceProvider, factoryName, httpClientName);
                return configure(serviceProvider, factoryCustomizer).Build();
            }).AddHttpClient(httpClientName);
        }

        private static INClientFactoryCustomizer<HttpRequestMessage, HttpResponseMessage> CreatePreConfiguredCustomizer(IServiceProvider serviceProvider, string factoryName, string httpClientName)
        {
            return new NClientFactoryBuilder()
                .For(factoryName)
                .TrySetSystemHttpClient(serviceProvider, httpClientName)
                .TrySetLogging(serviceProvider);
        }
    }
}
