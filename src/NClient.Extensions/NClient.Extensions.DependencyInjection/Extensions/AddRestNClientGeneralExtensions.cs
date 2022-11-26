using System;
using System.Net.Http;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using NClient.Common.Helpers;
using NClient.Core.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    public static partial class AddRestNClientExtensions
    {
        internal static IGuidProvider GuidProvider { get; set; } = new GuidProvider();

        /// <summary>Adds a NClient client to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="serviceLifetime">Specifies the lifetime of a service in an <see cref="IServiceCollection"/></param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClient<TClient>(this IServiceCollection serviceCollection,
            string host, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            
            return AddRestNClient<TClient>(serviceCollection, new Uri(host), builder => builder.Build(), serviceLifetime);
        }
        
        /// <summary>Adds a NClient client to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="serviceLifetime">Specifies the lifetime of a service in an <see cref="IServiceCollection"/></param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClient<TClient>(this IServiceCollection serviceCollection,
            Uri host, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            
            return AddRestNClient<TClient>(serviceCollection, host, builder => builder.Build(), serviceLifetime);
        }
        
        /// <summary>Adds a NClient client to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <param name="serviceLifetime">Specifies the lifetime of a service in an <see cref="IServiceCollection"/></param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClient<TClient>(this IServiceCollection serviceCollection,
            string host, 
            Func<INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage>, TClient> implementationFactory,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            
            return AddRestNClient(serviceCollection, new Uri(host), implementationFactory, serviceLifetime);
        }

        /// <summary>Adds a NClient client to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <param name="serviceLifetime">Specifies the lifetime of a service in an <see cref="IServiceCollection"/></param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClient<TClient>(this IServiceCollection serviceCollection,
            Uri host, 
            Func<INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage>, TClient> implementationFactory,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            var internalClientName = GuidProvider.Create().ToString();
            
            var serviceDescriptor = new ServiceDescriptor(typeof(TClient), serviceProvider =>
            {
                return implementationFactory(
                    new RestNClientBuilder(internalClientName, serviceProvider)
                        .For<TClient>(host));
            }, serviceLifetime);
            serviceCollection.Add(serviceDescriptor);
            return serviceCollection.AddHttpClient(internalClientName).ConfigureHttpClient(ConfigureDefaultHttpClient);
        }
        
        /// <summary>Adds a NClient client to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <param name="serviceLifetime">Specifies the lifetime of a service in an <see cref="IServiceCollection"/></param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClient<TClient>(this IServiceCollection serviceCollection,
            string host, 
            Func<IServiceProvider, INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage>, TClient> implementationFactory,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            
            return AddRestNClient(serviceCollection, new Uri(host), implementationFactory, serviceLifetime);
        }

        /// <summary>Adds a NClient client to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <param name="serviceLifetime">Specifies the lifetime of a service in an <see cref="IServiceCollection"/></param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClient<TClient>(this IServiceCollection serviceCollection,
            Uri host, 
            Func<IServiceProvider, INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage>, TClient> implementationFactory,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            
            var internalClientName = GuidProvider.Create().ToString();
            
            var serviceDescriptor = new ServiceDescriptor(typeof(TClient), serviceProvider =>
            {
                return implementationFactory(serviceProvider, 
                    new RestNClientBuilder(internalClientName, serviceProvider)
                        .For<TClient>(host));
            }, serviceLifetime);
            serviceCollection.Add(serviceDescriptor);
            return serviceCollection.AddHttpClient(internalClientName).ConfigureHttpClient(ConfigureDefaultHttpClient);
        }
        
        /// <summary>Adds a NClient client to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <param name="serviceLifetime">Specifies the lifetime of a service in an <see cref="IServiceCollection"/></param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClient<TClient>(this IServiceCollection serviceCollection,
            Func<IRestNClientBuilder, TClient> implementationFactory,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            var internalClientName = GuidProvider.Create().ToString();
            
            var serviceDescriptor = new ServiceDescriptor(typeof(TClient), serviceProvider =>
            {
                return implementationFactory(new RestNClientBuilder(internalClientName, serviceProvider));
            }, serviceLifetime);
            serviceCollection.Add(serviceDescriptor);
            return serviceCollection.AddHttpClient(internalClientName).ConfigureHttpClient(ConfigureDefaultHttpClient);
        }
        
        /// <summary>Adds a NClient client to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <param name="serviceLifetime">Specifies the lifetime of a service in an <see cref="IServiceCollection"/></param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IHttpClientBuilder AddRestNClient<TClient>(this IServiceCollection serviceCollection,
            Func<IServiceProvider, IRestNClientBuilder, TClient> implementationFactory,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            var internalClientName = GuidProvider.Create().ToString();
            
            var serviceDescriptor = new ServiceDescriptor(typeof(TClient), serviceProvider =>
            {
                return implementationFactory(serviceProvider, new RestNClientBuilder(internalClientName, serviceProvider));
            }, serviceLifetime);
            serviceCollection.Add(serviceDescriptor);
            return serviceCollection.AddHttpClient(internalClientName).ConfigureHttpClient(ConfigureDefaultHttpClient);
        }

        private static void ConfigureDefaultHttpClient(HttpClient httpClient)
        {
            httpClient.Timeout = Timeout.InfiniteTimeSpan;
        }
    }
}
