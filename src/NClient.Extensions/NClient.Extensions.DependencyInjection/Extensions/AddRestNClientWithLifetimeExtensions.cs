using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    public static partial class AddRestNClientExtensions
    {
        #region WhereHostString

        /// <summary>Adds a NClient client to the DI container as singleton.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientSingleton<TClient>(this IServiceCollection serviceCollection,
            string host)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            
            return serviceCollection.AddRestNClient<TClient>(host);
        }
        
        /// <summary>Adds a NClient client to the DI container as transient.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientTransient<TClient>(this IServiceCollection serviceCollection,
            string host)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            
            return serviceCollection.AddRestNClient<TClient>(host, ServiceLifetime.Transient);
        }
        
        /// <summary>Adds a NClient client to the DI container as scoped.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientScoped<TClient>(this IServiceCollection serviceCollection,
            string host)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            
            return serviceCollection.AddRestNClient<TClient>(host, ServiceLifetime.Scoped);
        }
        
        #endregion

        #region WhereHostURI
        
        /// <summary>Adds a NClient client to the DI container as singleton.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientSingleton<TClient>(this IServiceCollection serviceCollection, Uri host)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            
            return serviceCollection.AddRestNClient<TClient>(host);
        }
        
        /// <summary>Adds a NClient client to the DI container as transient.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientTransient<TClient>(this IServiceCollection serviceCollection, Uri host)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            
            return serviceCollection.AddRestNClient<TClient>(host, ServiceLifetime.Transient);
        }
        
        /// <summary>Adds a NClient client to the DI container as scoped.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientScoped<TClient>(this IServiceCollection serviceCollection, Uri host)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            
            return serviceCollection.AddRestNClient<TClient>(host, ServiceLifetime.Scoped);
        }

        #endregion

        #region WhereHostStringWithImplementationFactory
        
        /// <summary>Adds a NClient client to the DI container as singleton.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientSingleton<TClient>(this IServiceCollection serviceCollection,
            string host, 
            Func<INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            
            return serviceCollection.AddRestNClient(host, implementationFactory);
        }
        
        /// <summary>Adds a NClient client to the DI container as transient.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientTransient<TClient>(this IServiceCollection serviceCollection,
            string host, 
            Func<INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            
            return serviceCollection.AddRestNClient(host, implementationFactory, ServiceLifetime.Transient);
        }
        
        /// <summary>Adds a NClient client to the DI container as scoped.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientScoped<TClient>(this IServiceCollection serviceCollection,
            string host, 
            Func<INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            
            return serviceCollection.AddRestNClient(host, implementationFactory, ServiceLifetime.Scoped);
        }
        
        /// <summary>Adds a NClient client to the DI container as singleton.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientSingleton<TClient>(this IServiceCollection serviceCollection,
            string host, 
            Func<IServiceProvider, INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            
            return serviceCollection.AddRestNClient(host, implementationFactory);
        }
        
        /// <summary>Adds a NClient client to the DI container as transient.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientTransient<TClient>(this IServiceCollection serviceCollection,
            string host, 
            Func<IServiceProvider, INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            
            return serviceCollection.AddRestNClient(host, implementationFactory, ServiceLifetime.Transient);
        }
        
        /// <summary>Adds a NClient client to the DI container as scoped.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientScoped<TClient>(this IServiceCollection serviceCollection,
            string host, 
            Func<IServiceProvider, INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            
            return serviceCollection.AddRestNClient(host, implementationFactory, ServiceLifetime.Scoped);
        }

        #endregion

        #region WhereHostURIWIthImplementationFactory

        /// <summary>Adds a NClient client to the DI container as singleton.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientSingleton<TClient>(this IServiceCollection serviceCollection,
            Uri host, 
            Func<INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));

            return serviceCollection.AddRestNClient(host, implementationFactory);
        }

        /// <summary>Adds a NClient client to the DI container as transient.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientTransient<TClient>(this IServiceCollection serviceCollection,
            Uri host, 
            Func<INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));

            return serviceCollection.AddRestNClient(host, implementationFactory, ServiceLifetime.Transient);
        }

        /// <summary>Adds a NClient client to the DI container as scoped.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientScoped<TClient>(this IServiceCollection serviceCollection,
            Uri host, 
            Func<INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));

            return serviceCollection.AddRestNClient(host, implementationFactory, ServiceLifetime.Scoped);
        }

        /// <summary>Adds a NClient client to the DI container as singleton.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientSingleton<TClient>(this IServiceCollection serviceCollection,
            Uri host, 
            Func<IServiceProvider, INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));

            return serviceCollection.AddRestNClient(host, implementationFactory);
        }

        /// <summary>Adds a NClient client to the DI container as transient.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientTransient<TClient>(this IServiceCollection serviceCollection,
            Uri host, 
            Func<IServiceProvider, INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));

            return serviceCollection.AddRestNClient(host, implementationFactory, ServiceLifetime.Transient);
        }

        /// <summary>Adds a NClient client to the DI container as scoped.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientScoped<TClient>(this IServiceCollection serviceCollection,
            Uri host, 
            Func<IServiceProvider, INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage>, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));

            return serviceCollection.AddRestNClient(host, implementationFactory, ServiceLifetime.Scoped);
        }
        
        #endregion

        #region WithoutHost
        
        /// <summary>Adds a NClient client to the DI container as singleton.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientSingleton<TClient>(this IServiceCollection serviceCollection,
            Func<IRestNClientBuilder, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return serviceCollection.AddRestNClient(implementationFactory);
        }
        
        /// <summary>Adds a NClient client to the DI container as transient.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientTransient<TClient>(this IServiceCollection serviceCollection,
            Func<IRestNClientBuilder, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return serviceCollection.AddRestNClient(implementationFactory, ServiceLifetime.Transient);
        }
        
        /// <summary>Adds a NClient client to the DI container as scoped.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientScoped<TClient>(this IServiceCollection serviceCollection,
            Func<IRestNClientBuilder, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return serviceCollection.AddRestNClient(implementationFactory, ServiceLifetime.Scoped);
        }
        
        /// <summary>Adds a NClient client to the DI container as singleton.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientSingleton<TClient>(this IServiceCollection serviceCollection,
            Func<IServiceProvider, IRestNClientBuilder, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return serviceCollection.AddRestNClient(implementationFactory);
        }
        
        /// <summary>Adds a NClient client to the DI container as transient.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientTransient<TClient>(this IServiceCollection serviceCollection,
            Func<IServiceProvider, IRestNClientBuilder, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return serviceCollection.AddRestNClient(implementationFactory, ServiceLifetime.Transient);
        }
        
        /// <summary>Adds a NClient client to the DI container as scoped.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClientScoped<TClient>(this IServiceCollection serviceCollection,
            Func<IServiceProvider, IRestNClientBuilder, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));

            return serviceCollection.AddRestNClient(implementationFactory, ServiceLifetime.Scoped);
        }
        
        #endregion
    }
}
