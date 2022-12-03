using System;
using Microsoft.Extensions.DependencyInjection;
using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    public static partial class AddCustomNClientExtensions
    {
        #region WhereHostString
        
        /// <summary>Adds a NClient client to the DI container as singleton.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClientSingleton<TClient>(this IServiceCollection serviceCollection, 
            string host, Func<INClientApiBuilder<TClient>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            return serviceCollection.AddCustomNClient(host, implementationFactory);
        }
        
        /// <summary>Adds a NClient client to the DI container as transient.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClientTransient<TClient>(this IServiceCollection serviceCollection, 
            string host, Func<INClientApiBuilder<TClient>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            return serviceCollection.AddCustomNClient(host, implementationFactory, ServiceLifetime.Transient);
        }
        
        /// <summary>Adds a NClient client to the DI container as scoped.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClientScoped<TClient>(this IServiceCollection serviceCollection, 
            string host, Func<INClientApiBuilder<TClient>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            return serviceCollection.AddCustomNClient(host, implementationFactory, ServiceLifetime.Scoped);
        }
        
        /// <summary>Adds a NClient client to the DI container as singleton.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClientSingleton<TClient>(this IServiceCollection serviceCollection, 
            string host, Func<IServiceProvider, INClientApiBuilder<TClient>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return serviceCollection.AddCustomNClient(host, implementationFactory);
        }
        
        /// <summary>Adds a NClient client to the DI container as transient.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClientTransient<TClient>(this IServiceCollection serviceCollection, 
            string host, Func<IServiceProvider, INClientApiBuilder<TClient>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return serviceCollection.AddCustomNClient(host, implementationFactory, ServiceLifetime.Transient);
        }
        
        /// <summary>Adds a NClient client to the DI container as scoped.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClientScoped<TClient>(this IServiceCollection serviceCollection, 
            string host, Func<IServiceProvider, INClientApiBuilder<TClient>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return serviceCollection.AddCustomNClient(host, implementationFactory, ServiceLifetime.Scoped);
        }

        #endregion

        #region WhereHostURI
        
        /// <summary>Adds a NClient client to the DI container as singleton.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClientSingleton<TClient>(this IServiceCollection serviceCollection, 
            Uri host, Func<INClientApiBuilder<TClient>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return serviceCollection.AddCustomNClient(host, implementationFactory);
        }
        
        /// <summary>Adds a NClient client to the DI container as transient.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClientTransient<TClient>(this IServiceCollection serviceCollection, 
            Uri host, Func<INClientApiBuilder<TClient>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return serviceCollection.AddCustomNClient(host, implementationFactory, ServiceLifetime.Transient);
        }
        
        /// <summary>Adds a NClient client to the DI container as scoped.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClientScoped<TClient>(this IServiceCollection serviceCollection, 
            Uri host, Func<INClientApiBuilder<TClient>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return serviceCollection.AddCustomNClient(host, implementationFactory, ServiceLifetime.Scoped);
        }
        
        /// <summary>Adds a NClient client to the DI container as singleton.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClientSingleton<TClient>(this IServiceCollection serviceCollection, 
            Uri host, Func<IServiceProvider, INClientApiBuilder<TClient>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            return serviceCollection.AddCustomNClient(host, implementationFactory);
        }
        
        /// <summary>Adds a NClient client to the DI container as transient.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClientTransient<TClient>(this IServiceCollection serviceCollection, 
            Uri host, Func<IServiceProvider, INClientApiBuilder<TClient>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            return serviceCollection.AddCustomNClient(host, implementationFactory, ServiceLifetime.Transient);
        }
        
        /// <summary>Adds a NClient client to the DI container as scoped.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClientScoped<TClient>(this IServiceCollection serviceCollection, 
            Uri host, Func<IServiceProvider, INClientApiBuilder<TClient>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            return serviceCollection.AddCustomNClient(host, implementationFactory, ServiceLifetime.Scoped);
        }
     
        #endregion

        #region WithoutHost
        
        /// <summary>Adds a NClient client to the DI container as singleton.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClientSingleton<TClient>(this IServiceCollection serviceCollection, 
            Func<INClientBuilder, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            return serviceCollection.AddCustomNClient(implementationFactory);
        }
        
        /// <summary>Adds a NClient client to the DI container as transient.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClientTransient<TClient>(this IServiceCollection serviceCollection, 
            Func<INClientBuilder, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            return serviceCollection.AddCustomNClient(implementationFactory, ServiceLifetime.Transient);
        }
        
        /// <summary>Adds a NClient client to the DI container as scoped.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClientScoped<TClient>(this IServiceCollection serviceCollection, 
            Func<INClientBuilder, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            return serviceCollection.AddCustomNClient(implementationFactory, ServiceLifetime.Scoped);
        }
        
        /// <summary>Adds a NClient client to the DI container as singleton.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClientSingleton<TClient>(this IServiceCollection serviceCollection,
            Func<IServiceProvider, INClientBuilder, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            return serviceCollection.AddCustomNClient(implementationFactory);
        }
        
        /// <summary>Adds a NClient client to the DI container as transient.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClientTransient<TClient>(this IServiceCollection serviceCollection,
            Func<IServiceProvider, INClientBuilder, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            return serviceCollection.AddCustomNClient(implementationFactory, ServiceLifetime.Transient);
        }
        
        /// <summary>Adds a NClient client to the DI container as scoped.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to create client with builder.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IServiceCollection AddCustomNClientScoped<TClient>(this IServiceCollection serviceCollection,
            Func<IServiceProvider, INClientBuilder, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            return serviceCollection.AddCustomNClient(implementationFactory, ServiceLifetime.Scoped);
        }
        
        #endregion
    }
}
