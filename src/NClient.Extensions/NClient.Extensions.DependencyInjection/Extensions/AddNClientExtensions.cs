using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions.Builders;
using NClient.Common.Helpers;
using NClient.Core.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    public static class AddNClientExtensions
    {
        private static readonly IGuidProvider GuidProvider = new GuidProvider();
        
        /// <summary>
        /// Adds a NClient client to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddNClient<TClient>(this IServiceCollection serviceCollection,
            string host)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));

            var httpClientName = GuidProvider.Create().ToString();
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var preConfiguredBuilder = CreatePreConfiguredBuilder<TClient>(serviceProvider, host, httpClientName);
                return preConfiguredBuilder.Build();
            }).AddHttpClient(httpClientName);
        }

        /// <summary>
        /// Adds a NClient client to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="configure">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddNClient<TClient>(this IServiceCollection serviceCollection,
            string host, Func<INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage>, TClient> configure)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(configure, nameof(configure));

            var httpClientName = GuidProvider.Create().ToString();
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var preConfiguredBuilder = CreatePreConfiguredBuilder<TClient>(serviceProvider, host, httpClientName);
                return configure(preConfiguredBuilder);
            }).AddHttpClient(httpClientName);
        }

        /// <summary>
        /// Adds a NClient client to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="configure">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddNClient<TClient>(this IServiceCollection serviceCollection,
            string host, Func<IServiceProvider, INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage>, TClient> configure)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(configure, nameof(configure));

            var httpClientName = GuidProvider.Create().ToString();
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var preConfiguredBuilder = CreatePreConfiguredBuilder<TClient>(serviceProvider, host, httpClientName);
                return configure(serviceProvider, preConfiguredBuilder);
            }).AddHttpClient(httpClientName);
        }

        private static INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> CreatePreConfiguredBuilder<TClient>(
            IServiceProvider serviceProvider, string host, string? httpClientName)
            where TClient : class
        {
            return new AspNetNClientBuilder(httpClientName, serviceProvider).For<TClient>(host);
        }
    }
}
