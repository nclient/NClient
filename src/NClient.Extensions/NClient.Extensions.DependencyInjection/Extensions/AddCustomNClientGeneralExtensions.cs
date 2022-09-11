using System;
using Microsoft.Extensions.DependencyInjection;
using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    public static partial class AddCustomNClientExtensions
    {
        /// <summary>Adds a NClient client to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <param name="serviceLifetime">Specifies the lifetime of a service in an <see cref="IServiceCollection"/></param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClient<TClient>(this IServiceCollection serviceCollection, 
            string host, Func<INClientApiBuilder<TClient>, TClient> implementationFactory,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            return AddCustomNClient(serviceCollection, new Uri(host), implementationFactory, serviceLifetime);
        }
        
        /// <summary>Adds a NClient client to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <param name="serviceLifetime">Specifies the lifetime of a service in an <see cref="IServiceCollection"/></param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClient<TClient>(this IServiceCollection serviceCollection, 
            Uri host, Func<INClientApiBuilder<TClient>, TClient> implementationFactory,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            var serviceDescriptor = new ServiceDescriptor(typeof(TClient), _ => implementationFactory(new NClientBuilder().For<TClient>(host)), serviceLifetime);
            serviceCollection.Add(serviceDescriptor);
            return serviceCollection;
        }
        
        /// <summary>Adds a NClient client to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <param name="serviceLifetime">Specifies the lifetime of a service in an <see cref="IServiceCollection"/></param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClient<TClient>(this IServiceCollection serviceCollection, 
            string host, Func<IServiceProvider, INClientApiBuilder<TClient>, TClient> implementationFactory,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return AddCustomNClient(serviceCollection, new Uri(host), implementationFactory, serviceLifetime);
        }
        
        /// <summary>Adds a NClient client to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <param name="serviceLifetime">Specifies the lifetime of a service in an <see cref="IServiceCollection"/></param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClient<TClient>(this IServiceCollection serviceCollection, 
            Uri host, Func<IServiceProvider, INClientApiBuilder<TClient>, TClient> implementationFactory,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            var serviceDescriptor = new ServiceDescriptor(typeof(TClient), serviceProvider => implementationFactory(serviceProvider, new NClientBuilder().For<TClient>(host)), serviceLifetime);
            serviceCollection.Add(serviceDescriptor);
            return serviceCollection;
        }
        
        /// <summary>Adds a NClient client to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <param name="serviceLifetime">Specifies the lifetime of a service in an <see cref="IServiceCollection"/></param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClient<TClient>(this IServiceCollection serviceCollection, 
            Func<INClientBuilder, TClient> implementationFactory,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            var serviceDescriptor = new ServiceDescriptor(typeof(TClient), _ => implementationFactory(new NClientBuilder()), serviceLifetime);
            serviceCollection.Add(serviceDescriptor);
            return serviceCollection;
        }
        
        /// <summary>Adds a NClient client to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <param name="serviceLifetime">Specifies the lifetime of a service in an <see cref="IServiceCollection"/></param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClient<TClient>(this IServiceCollection serviceCollection,
            Func<IServiceProvider, INClientBuilder, TClient> implementationFactory,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            var serviceDescriptor = new ServiceDescriptor(typeof(TClient), serviceProvider => implementationFactory(serviceProvider, new NClientBuilder()), serviceLifetime);
            serviceCollection.Add(serviceDescriptor);
            return serviceCollection;
        }
    }
}
