using System;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions;
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
                var nclientFactoryBuilder = PreBuild(serviceProvider, httpClientName);
                return nclientFactoryBuilder.Build();
            });
        }

        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configure">The action to configure NClient settings.</param>
        /// <param name="httpClientName">The logical name of the HttpClient to create.</param>
        public static IServiceCollection AddNClientFactory(this IServiceCollection serviceCollection,
            Func<IOptionalNClientFactoryBuilder, IOptionalNClientFactoryBuilder> configure, string? httpClientName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(configure, nameof(configure));

            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var nclientFactoryBuilder = PreBuild(serviceProvider, httpClientName);
                return configure(nclientFactoryBuilder).Build();
            });
        }

        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configure">The action to configure NClient settings.</param>
        /// <param name="httpClientName">The logical name of the HttpClient to create.</param>
        public static IServiceCollection AddNClientFactory(this IServiceCollection serviceCollection,
            Func<IServiceProvider, IOptionalNClientFactoryBuilder, IOptionalNClientFactoryBuilder> configure, string? httpClientName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(configure, nameof(configure));

            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var nclientFactoryBuilder = PreBuild(serviceProvider, httpClientName);
                return configure(serviceProvider, nclientFactoryBuilder).Build();
            });
        }

        private static IOptionalNClientFactoryBuilder PreBuild(
            IServiceProvider serviceProvider, string? httpClientName)
        {
            return new NClientFactoryBuilder()
                .WithRegisteredProviders(serviceProvider, httpClientName);
        }
    }
}