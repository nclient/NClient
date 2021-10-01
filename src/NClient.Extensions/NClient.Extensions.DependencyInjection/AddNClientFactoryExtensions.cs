using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions.Customization;
using NClient.Common.Helpers;
using NClient.Extensions.DependencyInjection.Extensions;

namespace NClient.Extensions.DependencyInjection
{
    public static class AddNClientFactoryExtensions
    {
        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="httpClientName">The logical name of the HttpClient to create.</param>
        public static IServiceCollection AddNClientFactory(this IServiceCollection serviceCollection,
            string? httpClientName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));

            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var factoryCustomizer = CreateCustomizer(serviceProvider, httpClientName);
                return factoryCustomizer.Build();
            });
        }

        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configure">The action to configure NClient settings.</param>
        /// <param name="httpClientName">The logical name of the HttpClient to create.</param>
        public static IServiceCollection AddNClientFactory(this IServiceCollection serviceCollection,
            Func<INClientFactoryCustomizer<HttpRequestMessage, HttpResponseMessage>, INClientFactoryCustomizer<HttpRequestMessage, HttpResponseMessage>> configure, string? httpClientName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(configure, nameof(configure));

            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var factoryCustomizer = CreateCustomizer(serviceProvider, httpClientName);
                return configure(factoryCustomizer).Build();
            });
        }

        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configure">The action to configure NClient settings.</param>
        /// <param name="httpClientName">The logical name of the HttpClient to create.</param>
        public static IServiceCollection AddNClientFactory(this IServiceCollection serviceCollection,
            Func<IServiceProvider, INClientFactoryCustomizer<HttpRequestMessage, HttpResponseMessage>, INClientFactoryCustomizer<HttpRequestMessage, HttpResponseMessage>> configure, string? httpClientName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(configure, nameof(configure));

            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var factoryCustomizer = CreateCustomizer(serviceProvider, httpClientName);
                return configure(serviceProvider, factoryCustomizer).Build();
            });
        }

        private static INClientFactoryCustomizer<HttpRequestMessage, HttpResponseMessage> CreateCustomizer(IServiceProvider serviceProvider, string? httpClientName)
        {
            return new NClientFactoryBuilder()
                .Use()
                .WithRegisteredProviders(serviceProvider, httpClientName);
        }
    }
}
