using System;
using System.Net.Http;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using NClient.Common.Helpers;
using NClient.Core.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    public static class AddRestNClientExtensions
    {
        private static readonly IGuidProvider GuidProvider = new GuidProvider();
        
        /// <summary>
        /// Adds a NClient client to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IDiNClientBuilder<TClient, HttpRequestMessage, HttpResponseMessage> AddRestNClient<TClient>(
            this IServiceCollection serviceCollection,
            string host)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));

            return AddRestNClient<TClient>(serviceCollection, _ => host);
        }
        
        /// <summary>
        /// Adds a NClient client to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="hostProvider">The provider returning base address of URI used when sending requests.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IDiNClientBuilder<TClient, HttpRequestMessage, HttpResponseMessage> AddRestNClient<TClient>(
            this IServiceCollection serviceCollection,
            Func<IServiceProvider, string> hostProvider)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(hostProvider, nameof(hostProvider));

            var httpClientName = GuidProvider.Create().ToString();
            var httpClientBuilder = serviceCollection.AddHttpClient(httpClientName).ConfigureHttpClient(ConfigureDefaultHttpClient);
            serviceCollection.AddSingleton(serviceProvider =>
            {
                return new RestNClientBuilder(serviceProvider, httpClientName)
                    .For<TClient>(hostProvider.Invoke(serviceProvider))
                    .Build();
            });
            return new DiNClientBuilder<TClient, HttpRequestMessage, HttpResponseMessage>(httpClientBuilder);
        }

        private static void ConfigureDefaultHttpClient(HttpClient httpClient)
        {
            httpClient.Timeout = Timeout.InfiniteTimeSpan;
        }
    }
}
