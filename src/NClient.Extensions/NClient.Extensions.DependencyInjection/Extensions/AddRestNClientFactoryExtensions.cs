using System;
using System.Net.Http;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using NClient.Common.Helpers;
using NClient.Core.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    public static class AddRestNClientFactoryExtensions
    {
        internal static IGuidProvider GuidProvider { get; } = new GuidProvider();
        
        /// <summary>Adds a NClient factory to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="factoryName">The name of the factory.</param>
        public static IDiNClientFactoryBuilder<HttpRequestMessage, HttpResponseMessage> AddRestNClientFactory(
            this IServiceCollection serviceCollection,
            string factoryName)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNullOrEmpty(factoryName, nameof(factoryName));

            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            
            var internalFactoryName = GuidProvider.Create().ToString();
            var httpClientBuilder = serviceCollection.AddHttpClient(internalFactoryName).ConfigureHttpClient(ConfigureDefaultHttpClient);
            serviceCollection.AddSingleton(serviceProvider =>
            {
                return new RestNClientFactoryBuilder(internalFactoryName, serviceProvider)
                    .For(factoryName)
                    .Build();
            });
            return new DiNClientFactoryBuilder<HttpRequestMessage, HttpResponseMessage>(httpClientBuilder);
        }

        /// <summary>Adds a NClient factory to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to create client factory with builder.</param>
        public static IDiNClientFactoryBuilder<HttpRequestMessage, HttpResponseMessage> AddRestNClientFactory(
            this IServiceCollection serviceCollection,
            Func<IRestNClientFactoryBuilder, INClientFactory> implementationFactory)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            var internalFactoryName = GuidProvider.Create().ToString();
            var httpClientBuilder = serviceCollection.AddHttpClient(internalFactoryName).ConfigureHttpClient(ConfigureDefaultHttpClient);
            serviceCollection.AddSingleton(serviceProvider =>
            {
                return implementationFactory(new RestNClientFactoryBuilder(internalFactoryName, serviceProvider));
            });
            return new DiNClientFactoryBuilder<HttpRequestMessage, HttpResponseMessage>(httpClientBuilder);
        }
        
        /// <summary>Adds a NClient factory to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to create client factory with builder.</param>
        public static IDiNClientFactoryBuilder<HttpRequestMessage, HttpResponseMessage> AddRestNClientFactory(
            this IServiceCollection serviceCollection,
            Func<IServiceProvider, IRestNClientFactoryBuilder, INClientFactory> implementationFactory)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            var internalFactoryName = GuidProvider.Create().ToString();
            var httpClientBuilder = serviceCollection.AddHttpClient(internalFactoryName).ConfigureHttpClient(ConfigureDefaultHttpClient);
            serviceCollection.AddSingleton(serviceProvider =>
            {
                return implementationFactory(serviceProvider, new RestNClientFactoryBuilder(internalFactoryName, serviceProvider));
            });
            return new DiNClientFactoryBuilder<HttpRequestMessage, HttpResponseMessage>(httpClientBuilder);
        }

        private static void ConfigureDefaultHttpClient(HttpClient httpClient)
        {
            httpClient.Timeout = Timeout.InfiniteTimeSpan;
        }
    }
}
