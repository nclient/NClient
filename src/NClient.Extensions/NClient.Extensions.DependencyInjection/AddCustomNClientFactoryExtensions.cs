﻿using System;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions;
using NClient.Abstractions.Builders;
using NClient.Common.Helpers;
using NClient.Core.Helpers;

namespace NClient.Extensions.DependencyInjection
{
    public static class AddCustomNClientFactoryExtensions
    {
        private static readonly IGuidProvider GuidProvider = new GuidProvider();

        // TODO: doc
        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configure">The action to configure NClient settings.</param>
        /// <param name="factoryName">The name of the factory.</param>
        public static IServiceCollection AddCustomNClientFactory(this IServiceCollection serviceCollection,
            Func<INClientFactoryHttpClientBuilder, INClientFactory> configure,
            string? factoryName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(configure, nameof(configure));

            factoryName ??= GuidProvider.Create().ToString();
            
            return serviceCollection.AddSingleton(_ => configure(new CustomNClientFactoryBuilder().For(factoryName)));
        }

        // TODO: doc
        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configure">The action to configure NClient settings.</param>
        /// <param name="factoryName">The name of the factory.</param>
        public static IServiceCollection AddCustomNClientFactory(this IServiceCollection serviceCollection,
            Func<IServiceProvider, INClientFactoryHttpClientBuilder, INClientFactory> configure,
            string? factoryName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(configure, nameof(configure));

            factoryName ??= GuidProvider.Create().ToString();

            return serviceCollection.AddSingleton(serviceProvider => configure(serviceProvider, new CustomNClientFactoryBuilder().For(factoryName)));
        }
    }
}
