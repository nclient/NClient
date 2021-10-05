using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions;
using NClient.Abstractions.Builders;
using NClient.Common.Helpers;
using NClient.Core.Helpers;

// ReSharper disable once CheckNamespace
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
                var preConfiguredBuilder = CreatePreConfiguredBuilder(serviceProvider, factoryName, httpClientName);
                return preConfiguredBuilder.Build();
            }).AddHttpClient(httpClientName);
        }

        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <param name="factoryName">The name of the factory.</param>
        public static IHttpClientBuilder AddNClientFactory(this IServiceCollection serviceCollection,
            Func<INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage>, INClientFactory> implementationFactory,
            string? factoryName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            factoryName ??= GuidProvider.Create().ToString();
            var httpClientName = GuidProvider.Create().ToString();
            
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var preConfiguredBuilder = CreatePreConfiguredBuilder(serviceProvider, factoryName, httpClientName);
                return implementationFactory(preConfiguredBuilder);
            }).AddHttpClient(httpClientName);
        }

        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <param name="factoryName">The name of the factory.</param>
        public static IHttpClientBuilder AddNClientFactory(this IServiceCollection serviceCollection,
            Func<IServiceProvider, INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage>, INClientFactory> implementationFactory,
            string? factoryName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            factoryName ??= GuidProvider.Create().ToString();
            var httpClientName = GuidProvider.Create().ToString();

            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var preConfiguredBuilder = CreatePreConfiguredBuilder(serviceProvider, factoryName, httpClientName);
                return implementationFactory(serviceProvider, preConfiguredBuilder);
            }).AddHttpClient(httpClientName);
        }

        private static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> CreatePreConfiguredBuilder(IServiceProvider serviceProvider, string factoryName, string httpClientName)
        {
            return new InjectedFactoryBuilder(serviceProvider, httpClientName).For(factoryName);
        }
    }
}
