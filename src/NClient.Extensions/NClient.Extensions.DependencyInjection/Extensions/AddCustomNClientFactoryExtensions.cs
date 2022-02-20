using System;
using Microsoft.Extensions.DependencyInjection;
using NClient.Common.Helpers;
using NClient.Core.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    public static class AddCustomNClientFactoryExtensions
    {
        internal static IGuidProvider GuidProvider { get; set; } = new GuidProvider();
        
        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to configure NClient factory settings.</param>
        public static IServiceCollection AddCustomNClientFactory(this IServiceCollection serviceCollection,
            Func<INClientFactoryApiBuilder, INClientFactory> implementationFactory)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return AddCustomNClientFactory(
                serviceCollection,
                factoryName: GuidProvider.Create().ToString(),
                implementationFactory);
        }
        
        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to configure NClient factory settings.</param>
        public static IServiceCollection AddCustomNClientFactory(this IServiceCollection serviceCollection,
            Func<IServiceProvider, INClientFactoryApiBuilder, INClientFactory> implementationFactory)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return AddCustomNClientFactory(
                serviceCollection,
                factoryName: GuidProvider.Create().ToString(),
                implementationFactory);
        }

        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="factoryName">The name of the factory.</param>
        /// <param name="implementationFactory">The action to configure NClient factory settings.</param>
        public static IServiceCollection AddCustomNClientFactory(this IServiceCollection serviceCollection,
            string factoryName, Func<INClientFactoryApiBuilder, INClientFactory> implementationFactory)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNullOrEmpty(factoryName, nameof(factoryName));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return serviceCollection.AddSingleton(_ => implementationFactory(new NClientFactoryBuilder().For(factoryName)));
        }
        
        /// <summary>
        /// Adds a NClient factory to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="factoryName">The name of the factory.</param>
        /// <param name="implementationFactory">The action to configure NClient factory settings.</param>
        public static IServiceCollection AddCustomNClientFactory(this IServiceCollection serviceCollection,
            string factoryName, Func<IServiceProvider, INClientFactoryApiBuilder, INClientFactory> implementationFactory)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNullOrEmpty(factoryName, nameof(factoryName));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return serviceCollection.AddSingleton(serviceProvider => implementationFactory(serviceProvider, new NClientFactoryBuilder().For(factoryName)));
        }
    }
}
