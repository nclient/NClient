using System;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions.Customization;
using NClient.Common.Helpers;
using NClient.Core.Helpers;
using NClient.Extensions.DependencyInjection.Extensions;

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
        public static IServiceCollection AddCustomNClientFactory<TRequest, TResponse>(this IServiceCollection serviceCollection,
            Func<INClientFactoryCustomizer<TRequest, TResponse>, INClientFactoryCustomizer<TRequest, TResponse>> configure,
            string? factoryName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(configure, nameof(configure));

            factoryName ??= GuidProvider.Create().ToString();
            
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var factoryCustomizer = CreatePreConfiguredCustomizer<TRequest, TResponse>(serviceProvider, factoryName);
                return configure(factoryCustomizer).Build();
            });
        }

        // TODO: doc
        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configure">The action to configure NClient settings.</param>
        /// <param name="factoryName">The name of the factory.</param>
        public static IServiceCollection AddCustomNClientFactory<TRequest, TResponse>(this IServiceCollection serviceCollection,
            Func<IServiceProvider, INClientFactoryCustomizer<TRequest, TResponse>, INClientFactoryCustomizer<TRequest, TResponse>> configure,
            string? factoryName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(configure, nameof(configure));

            factoryName ??= GuidProvider.Create().ToString();

            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var factoryCustomizer = CreatePreConfiguredCustomizer<TRequest, TResponse>(serviceProvider, factoryName);
                return configure(serviceProvider, factoryCustomizer).Build();
            });
        }

        private static INClientFactoryCustomizer<TRequest, TResponse> CreatePreConfiguredCustomizer<TRequest, TResponse>(IServiceProvider serviceProvider, string factoryName)
        {
            return new NClientStandaloneFactoryBuilder<TRequest, TResponse>()
                .For(factoryName)
                .TrySetLogging(serviceProvider);
        }
    }
}
