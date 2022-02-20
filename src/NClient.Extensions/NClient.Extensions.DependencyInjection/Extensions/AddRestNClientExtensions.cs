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
        /// <param name="clientName">The client name.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IDiNClientBuilder<TClient, HttpRequestMessage, HttpResponseMessage> AddRestNClient<TClient>(
            this IServiceCollection serviceCollection,
            Uri host, string? clientName = null)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));

            return AddRestNClient<TClient>(serviceCollection, _ => host, clientName);
        }
        
        /// <summary>
        /// Adds a NClient client to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="hostFactory">The provider returning base address of URI used when sending requests.</param>
        /// <param name="clientName">The client name.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IDiNClientBuilder<TClient, HttpRequestMessage, HttpResponseMessage> AddRestNClient<TClient>(
            this IServiceCollection serviceCollection,
            Func<IServiceProvider, Uri> hostFactory, string? clientName = null)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(hostFactory, nameof(hostFactory));

            clientName ??= GuidProvider.Create().ToString();
            var httpClientBuilder = serviceCollection.AddHttpClient(clientName).ConfigureHttpClient(ConfigureDefaultHttpClient);
            serviceCollection.AddSingleton(serviceProvider =>
            {
                return new RestNClientBuilder(serviceProvider)
                    .For<TClient>(hostFactory.Invoke(serviceProvider), clientName)
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
