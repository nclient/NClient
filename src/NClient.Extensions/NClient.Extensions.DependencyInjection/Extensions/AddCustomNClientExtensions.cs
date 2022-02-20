using System;
using Microsoft.Extensions.DependencyInjection;
using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    public static class AddCustomNClientExtensions
    {
        /// <summary>
        /// Adds a NClient client to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClient<TClient>(this IServiceCollection serviceCollection, 
            Uri host, Func<INClientApiBuilder<TClient>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            return serviceCollection.AddSingleton(_ => implementationFactory(new NClientBuilder().For<TClient>(host)));
        }
        
        /// <summary>
        /// Adds a NClient client to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClient<TClient>(this IServiceCollection serviceCollection, 
            Uri host, Func<IServiceProvider, INClientApiBuilder<TClient>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            return serviceCollection.AddSingleton(serviceProvider => implementationFactory(serviceProvider, new NClientBuilder().For<TClient>(host)));
        }
        
        /// <summary>
        /// Adds a NClient client to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClient<TClient>(this IServiceCollection serviceCollection, 
            Func<NClientBuilder, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            return serviceCollection.AddSingleton(_ => implementationFactory(new NClientBuilder()));
        }
        
        /// <summary>
        /// Adds a NClient client to the DI container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClient<TClient>(this IServiceCollection serviceCollection,
            Func<IServiceProvider, NClientBuilder, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            return serviceCollection.AddSingleton(serviceProvider => implementationFactory(serviceProvider, new NClientBuilder()));
        }
    }
}
