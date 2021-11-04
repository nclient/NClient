using System;
using Microsoft.Extensions.DependencyInjection;
using NClient.Common.Helpers;
using NClient.Core.Helpers;

// ReSharper disable once CheckNamespace
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
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <param name="factoryName">The name of the factory.</param>
        public static IServiceCollection AddAdvancedNClientFactory(this IServiceCollection serviceCollection,
            Func<INClientFactoryAdvancedApiBuilder, INClientFactory> implementationFactory,
            string? factoryName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            factoryName ??= GuidProvider.Create().ToString();
            
            return serviceCollection.AddSingleton(_ => implementationFactory(new NClientFactoryAdvancedBuilder().For(factoryName)));
        }

        // TODO: doc
        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <param name="factoryName">The name of the factory.</param>
        public static IServiceCollection AddAdvancedNClientFactory(this IServiceCollection serviceCollection,
            Func<IServiceProvider, INClientFactoryAdvancedApiBuilder, INClientFactory> implementationFactory,
            string? factoryName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            factoryName ??= GuidProvider.Create().ToString();

            return serviceCollection.AddSingleton(serviceProvider => implementationFactory(serviceProvider, new NClientFactoryAdvancedBuilder().For(factoryName)));
        }
    }
}
