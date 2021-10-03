using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.Builders;
using NClient.Common.Helpers;
using NClient.Core.Helpers;
using NClient.Providers.HttpClient.System;

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
                var factoryCustomizer = CreatePreConfiguredCustomizer(serviceProvider, factoryName, httpClientName);
                return factoryCustomizer.Build();
            }).AddHttpClient(httpClientName);
        }

        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configure">The action to configure NClient settings.</param>
        /// <param name="factoryName">The name of the factory.</param>
        public static IHttpClientBuilder AddNClientFactory(this IServiceCollection serviceCollection,
            Func<INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage>, INClientFactory> configure,
            string? factoryName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(configure, nameof(configure));

            factoryName ??= GuidProvider.Create().ToString();
            var httpClientName = GuidProvider.Create().ToString();
            
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var factoryCustomizer = CreatePreConfiguredCustomizer(serviceProvider, factoryName, httpClientName);
                return configure(factoryCustomizer);
            }).AddHttpClient(httpClientName);
        }

        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configure">The action to configure NClient settings.</param>
        /// <param name="factoryName">The name of the factory.</param>
        public static IHttpClientBuilder AddNClientFactory(this IServiceCollection serviceCollection,
            Func<IServiceProvider, INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage>, INClientFactory> configure,
            string? factoryName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(configure, nameof(configure));

            factoryName ??= GuidProvider.Create().ToString();
            var httpClientName = GuidProvider.Create().ToString();

            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var factoryCustomizer = CreatePreConfiguredCustomizer(serviceProvider, factoryName, httpClientName);
                return configure(serviceProvider, factoryCustomizer);
            }).AddHttpClient(httpClientName);
        }

        private static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> CreatePreConfiguredCustomizer(IServiceProvider serviceProvider, string factoryName, string httpClientName)
        {
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            
            return new CustomNClientFactoryBuilder()
                .For(factoryName)
                .UsingSystemHttpClient(httpClientFactory, httpClientName)
                .UsingJsonSerializer()
                .WithLogging(loggerFactory);
        }
    }
}
