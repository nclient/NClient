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
        internal static IGuidProvider GuidProvider { get; set; } = new GuidProvider();
        
        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        public static IDiNClientFactoryBuilder<HttpRequestMessage, HttpResponseMessage> AddRestNClientFactory(
            this IServiceCollection serviceCollection)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));

            return AddRestNClientFactory(
                serviceCollection,
                factoryName: GuidProvider.Create().ToString());
        }
        
        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="factoryName">The name of the factory.</param>
        public static IDiNClientFactoryBuilder<HttpRequestMessage, HttpResponseMessage> AddRestNClientFactory(
            this IServiceCollection serviceCollection,
            string factoryName)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNullOrEmpty(factoryName, nameof(factoryName));
            
            var httpClientBuilder = serviceCollection.AddHttpClient(factoryName).ConfigureHttpClient(ConfigureDefaultHttpClient);
            serviceCollection.AddSingleton(serviceProvider =>
            {
                return new RestNClientFactoryBuilder(serviceProvider)
                    .For(factoryName)
                    .Build();
            });
            return new DiNClientFactoryBuilder<HttpRequestMessage, HttpResponseMessage>(httpClientBuilder);
        }

        private static void ConfigureDefaultHttpClient(HttpClient httpClient)
        {
            httpClient.Timeout = Timeout.InfiniteTimeSpan;
        }
    }
}
