using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Common.Helpers;

namespace NClient.Extensions.DependencyInjection
{
    public static class AddInterfaceBasedNClientExtensions
    {
        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection,
            string host, string? httpClientName = null)
            where TInterface : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var nclientBuilder = PreBuild<TInterface>(serviceProvider, host, httpClientName);
                return nclientBuilder.Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection,
            string host, Func<IInterfaceBasedClientBuilder<TInterface>, IInterfaceBasedClientBuilder<TInterface>> configure,
            string? httpClientName = null)
            where TInterface : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(configure, nameof(configure));
            
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var nclientBuilder = PreBuild<TInterface>(serviceProvider, host, httpClientName);
                return configure(nclientBuilder).Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection,
            string host, Func<IServiceProvider, IInterfaceBasedClientBuilder<TInterface>, IInterfaceBasedClientBuilder<TInterface>> configure,
            string? httpClientName = null)
            where TInterface : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(configure, nameof(configure));
            
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var nclientBuilder = PreBuild<TInterface>(serviceProvider, host, httpClientName);
                return configure(serviceProvider, nclientBuilder).Build();
            });
        }

        private static IInterfaceBasedClientBuilder<TInterface> PreBuild<TInterface>(
            IServiceProvider serviceProvider, string host, string? httpClientName)
            where TInterface : class
        {
            var nclientBuilder = new NClientBuilder()
                .Use<TInterface>(host);

            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            if (httpClientFactory is not null)
                nclientBuilder.WithCustomHttpClient(httpClientFactory, httpClientName);

            var logger = serviceProvider.GetService<ILogger<TInterface>>();
            if (logger is not null)
                nclientBuilder.WithLogging(logger);

            return nclientBuilder;
        }
    }
}
