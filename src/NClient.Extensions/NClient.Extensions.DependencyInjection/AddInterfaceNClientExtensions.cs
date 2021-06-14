﻿using System;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions;
using NClient.Common.Helpers;
using NClient.Extensions.DependencyInjection.Extensions;

namespace NClient.Extensions.DependencyInjection
{
    public static class AddInterfaceNClientExtensions
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
                var nclientBuilder = PreBuild<TInterface>(serviceProvider, host, httpClientName);
                return nclientBuilder.Build();
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
            string host, Func<IOptionalNClientBuilder<TInterface>, IOptionalNClientBuilder<TInterface>> configure,
            string? httpClientName = null)
            where TInterface : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(configure, nameof(configure));

            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var nclientBuilder = PreBuild<TInterface>(serviceProvider, host, httpClientName);
                return configure(nclientBuilder).Build();
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
            string host, Func<IServiceProvider, IOptionalNClientBuilder<TInterface>, IOptionalNClientBuilder<TInterface>> configure,
            string? httpClientName = null)
            where TInterface : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(configure, nameof(configure));

            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var nclientBuilder = PreBuild<TInterface>(serviceProvider, host, httpClientName);
                return configure(serviceProvider, nclientBuilder).Build();
            });
        }

        private static IOptionalNClientBuilder<TInterface> PreBuild<TInterface>(
            IServiceProvider serviceProvider, string host, string? httpClientName)
            where TInterface : class
        {
            return new NClientBuilder()
                .Use<TInterface>(host)
                .WithRegisteredProviders(serviceProvider, httpClientName); ;
        }
    }
}
