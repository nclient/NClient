﻿using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions.Customization;
using NClient.Common.Helpers;
using NClient.Core.Helpers;
using NClient.Extensions.DependencyInjection.Extensions;

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
                var builderCustomizer = CreatePreConfiguredCustomizer<TClient>(serviceProvider, host, httpClientName);
                return builderCustomizer.Build();
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
            string host, Func<INClientBuilderCustomizer<TClient, HttpRequestMessage, HttpResponseMessage>, INClientBuilderCustomizer<TClient, HttpRequestMessage, HttpResponseMessage>> configure)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(configure, nameof(configure));

            var httpClientName = GuidProvider.Create().ToString();
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var builderCustomizer = CreatePreConfiguredCustomizer<TClient>(serviceProvider, host, httpClientName);
                return configure(builderCustomizer).Build();
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
            string host, Func<IServiceProvider, INClientBuilderCustomizer<TClient, HttpRequestMessage, HttpResponseMessage>, INClientBuilderCustomizer<TClient, HttpRequestMessage, HttpResponseMessage>> configure)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(configure, nameof(configure));

            var httpClientName = GuidProvider.Create().ToString();
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var builderCustomizer = CreatePreConfiguredCustomizer<TClient>(serviceProvider, host, httpClientName);
                return configure(serviceProvider, builderCustomizer).Build();
            }).AddHttpClient(httpClientName);
        }

        private static INClientBuilderCustomizer<TClient, HttpRequestMessage, HttpResponseMessage> CreatePreConfiguredCustomizer<TClient>(
            IServiceProvider serviceProvider, string host, string? httpClientName)
            where TClient : class
        {
            return new NClientBuilder()
                .For<TClient>(host)
                .TrySetSystemHttpClient(serviceProvider, httpClientName)
                .TrySetLogging(serviceProvider);
        }
    }
}
