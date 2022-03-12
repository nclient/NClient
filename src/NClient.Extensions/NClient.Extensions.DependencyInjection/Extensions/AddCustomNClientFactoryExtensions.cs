using System;
using Microsoft.Extensions.DependencyInjection;
using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    public static class AddCustomNClientFactoryExtensions
    {
        /// <summary>Adds a NClient factory to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="factoryName">The name of the factory.</param>
        /// <param name="implementationFactory">The action to create client factory with builder.</param>
        public static IServiceCollection AddCustomNClientFactory(this IServiceCollection serviceCollection,
            string factoryName, Func<INClientFactoryApiBuilder, INClientFactory> implementationFactory)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNullOrEmpty(factoryName, nameof(factoryName));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return serviceCollection.AddSingleton(_ => implementationFactory(new NClientFactoryBuilder().For(factoryName)));
        }
        
        /// <summary>Adds a NClient factory to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="factoryName">The name of the factory.</param>
        /// <param name="implementationFactory">The action to create client factory with builder.</param>
        public static IServiceCollection AddCustomNClientFactory(this IServiceCollection serviceCollection,
            string factoryName, Func<IServiceProvider, INClientFactoryApiBuilder, INClientFactory> implementationFactory)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNullOrEmpty(factoryName, nameof(factoryName));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return serviceCollection.AddSingleton(serviceProvider => implementationFactory(serviceProvider, new NClientFactoryBuilder().For(factoryName)));
        }
        
        /// <summary>Adds a NClient factory to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to create client factory with builder.</param>
        public static IServiceCollection AddCustomNClientFactory(this IServiceCollection serviceCollection,
            Func<INClientFactoryBuilder, INClientFactory> implementationFactory)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return serviceCollection.AddSingleton(_ => implementationFactory(new NClientFactoryBuilder()));
        }
        
        /// <summary>Adds a NClient factory to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to create client factory with builder.</param>
        public static IServiceCollection AddCustomNClientFactory(this IServiceCollection serviceCollection,
            Func<IServiceProvider, INClientFactoryBuilder, INClientFactory> implementationFactory)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return serviceCollection.AddSingleton(serviceProvider => implementationFactory(serviceProvider, new NClientFactoryBuilder()));
        }
    }
}
