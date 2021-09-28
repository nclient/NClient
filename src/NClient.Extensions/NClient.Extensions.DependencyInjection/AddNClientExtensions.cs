using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions;
using NClient.Common.Helpers;
using NClient.Extensions.DependencyInjection.Extensions;

namespace NClient.Extensions.DependencyInjection
{
    public static class AddNClientExtensions
    {
        /// <summary>
        /// Adds a NClient client to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="httpClientName">The logical name of the HttpClient to create.</param>
        /// <typeparam name="TInterface">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection,
            string host, string? httpClientName = null)
            where TInterface : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));

            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var builderCustomizer = CreateCustomizer<TInterface>(serviceProvider, host, httpClientName);
                return builderCustomizer.Build();
            });
        }

        /// <summary>
        /// Adds a NClient client to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="configure">The action to configure NClient settings.</param>
        /// <param name="httpClientName">The logical name of the HttpClient to create.</param>
        /// <typeparam name="TInterface">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection,
            string host, Func<INClientBuilderCustomizer<TInterface, HttpRequestMessage, HttpResponseMessage>, INClientBuilderCustomizer<TInterface, HttpRequestMessage, HttpResponseMessage>> configure,
            string? httpClientName = null)
            where TInterface : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(configure, nameof(configure));

            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var builderCustomizer = CreateCustomizer<TInterface>(serviceProvider, host, httpClientName);
                return configure(builderCustomizer).Build();
            });
        }

        /// <summary>
        /// Adds a NClient client to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="configure">The action to configure NClient settings.</param>
        /// <param name="httpClientName">The logical name of the HttpClient to create.</param>
        /// <typeparam name="TInterface">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection,
            string host, Func<IServiceProvider, INClientBuilderCustomizer<TInterface, HttpRequestMessage, HttpResponseMessage>, INClientBuilderCustomizer<TInterface, HttpRequestMessage, HttpResponseMessage>> configure,
            string? httpClientName = null)
            where TInterface : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(configure, nameof(configure));

            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var builderCustomizer = CreateCustomizer<TInterface>(serviceProvider, host, httpClientName);
                return configure(serviceProvider, builderCustomizer).Build();
            });
        }

        private static INClientBuilderCustomizer<TInterface, HttpRequestMessage, HttpResponseMessage> CreateCustomizer<TInterface>(
            IServiceProvider serviceProvider, string host, string? httpClientName)
            where TInterface : class
        {
            return new NClientBuilder()
                .Use<TInterface>(host)
                .WithRegisteredProviders(serviceProvider, httpClientName);
        }
    }
}
