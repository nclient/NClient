using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;

namespace NClient.Extensions.DependencyInjection
{
    public static class AddInterfaceBasedNClientExtensions
    {
        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection,
            string host, string? httpClientName = null)
            where TInterface : class
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();

                return new NClientBuilder()
                    .Use<TInterface>(host)
                    .WithCustomHttpClient(httpClientFactory, httpClientName)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection,
            string host, Func<IInterfaceBasedClientBuilder<TInterface>, IInterfaceBasedClientBuilder<TInterface>> configure,
            string? httpClientName = null)
            where TInterface : class
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();

                var nclientBuilder = new NClientBuilder()
                    .Use<TInterface>(host)
                    .WithCustomHttpClient(httpClientFactory, httpClientName)
                    .WithLogging(logger);

                return configure(nclientBuilder).Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection,
            string host, Func<IServiceProvider, IInterfaceBasedClientBuilder<TInterface>, IInterfaceBasedClientBuilder<TInterface>> configure,
            string? httpClientName = null)
            where TInterface : class
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();

                var nclientBuilder = new NClientBuilder()
                    .Use<TInterface>(host)
                    .WithCustomHttpClient(httpClientFactory, httpClientName)
                    .WithLogging(logger);

                return configure(serviceProvider, nclientBuilder).Build();
            });
        }
    }
}
