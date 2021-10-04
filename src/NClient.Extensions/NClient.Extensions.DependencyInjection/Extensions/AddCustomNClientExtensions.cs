using System;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions.Builders;
using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    public static class AddCustomNClientExtensions
    {
        // TODO: doc
        /// <summary>
        /// Adds a NClient client to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="configure">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClient<TClient>(this IServiceCollection serviceCollection,
            string host, Func<INClientHttpClientBuilder<TClient>, TClient> configure)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(configure, nameof(configure));
            
            return serviceCollection.AddSingleton(_ => configure(new CustomNClientBuilder().For<TClient>(host)));
        }

        // TODO: doc
        /// <summary>
        /// Adds a NClient client to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="configure">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClient<TClient>(this IServiceCollection serviceCollection,
            string host, Func<IServiceProvider, INClientHttpClientBuilder<TClient>, TClient> configure)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(configure, nameof(configure));
            
            return serviceCollection.AddSingleton(serviceProvider => configure(serviceProvider, new CustomNClientBuilder().For<TClient>(host)));
        }
    }
}
