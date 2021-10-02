using System;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions.Customization;
using NClient.Common.Helpers;
using NClient.Extensions.DependencyInjection.Extensions;

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
        public static IServiceCollection AddCustomNClient<TClient, TRequest, TResponse>(this IServiceCollection serviceCollection,
            string host, Func<INClientBuilderCustomizer<TClient, TRequest, TResponse>, INClientBuilderCustomizer<TClient, TRequest, TResponse>> configure)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(configure, nameof(configure));
            
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var builderCustomizer = CreatePreConfiguredCustomizer<TClient, TRequest, TResponse>(serviceProvider, host);
                return configure(builderCustomizer).Build();
            });
        }

        // TODO: doc
        /// <summary>
        /// Adds a NClient client to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="configure">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClient<TClient, TRequest, TResponse>(this IServiceCollection serviceCollection,
            string host, Func<IServiceProvider, INClientBuilderCustomizer<TClient, TRequest, TResponse>, INClientBuilderCustomizer<TClient, TRequest, TResponse>> configure)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(configure, nameof(configure));
            
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var builderCustomizer = CreatePreConfiguredCustomizer<TClient, TRequest, TResponse>(serviceProvider, host);
                return configure(serviceProvider, builderCustomizer).Build();
            });
        }

        private static INClientBuilderCustomizer<TClient, TRequest, TResponse> CreatePreConfiguredCustomizer<TClient, TRequest, TResponse>(
            IServiceProvider serviceProvider, string host)
            where TClient : class
        {
            return new NClientStandaloneBuilder<TRequest, TResponse>()
                .For<TClient>(host)
                .TrySetLogging(serviceProvider);
        }
    }
}
