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
        public static IHttpClientBuilder AddRestNClientFactory(this IServiceCollection serviceCollection,
            string factoryName)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNullOrEmpty(factoryName, nameof(factoryName));

            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            
            var internalFactoryName = GuidProvider.Create().ToString();
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                return new RestNClientFactoryBuilder(internalFactoryName, serviceProvider)
                    .For(factoryName)
                    .Build();
            }).AddHttpClient(internalFactoryName).ConfigureHttpClient(ConfigureDefaultHttpClient);
        }

        /// <summary>Adds a NClient factory to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to create client factory with builder.</param>
        public static IHttpClientBuilder AddRestNClientFactory(this IServiceCollection serviceCollection,
            Func<IRestNClientFactoryBuilder, INClientFactory> implementationFactory)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            var internalFactoryName = GuidProvider.Create().ToString();
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                return implementationFactory(new RestNClientFactoryBuilder(internalFactoryName, serviceProvider));
            }).AddHttpClient(internalFactoryName).ConfigureHttpClient(ConfigureDefaultHttpClient);
        }
        
        /// <summary>Adds a NClient factory to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to create client factory with builder.</param>
        public static IHttpClientBuilder AddRestNClientFactory(this IServiceCollection serviceCollection,
            Func<IServiceProvider, IRestNClientFactoryBuilder, INClientFactory> implementationFactory)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            var internalFactoryName = GuidProvider.Create().ToString();
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                return implementationFactory(serviceProvider, new RestNClientFactoryBuilder(internalFactoryName, serviceProvider));
            }).AddHttpClient(internalFactoryName).ConfigureHttpClient(ConfigureDefaultHttpClient);
        }

        private static void ConfigureDefaultHttpClient(HttpClient httpClient)
        {
            httpClient.Timeout = Timeout.InfiniteTimeSpan;
        }
    }
}
