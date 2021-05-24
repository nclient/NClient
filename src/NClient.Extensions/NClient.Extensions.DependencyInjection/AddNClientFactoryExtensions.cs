using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;

namespace NClient.Extensions.DependencyInjection
{
    public static class AddNClientFactoryExtensions
    {
        public static IServiceCollection AddNClientFactory(this IServiceCollection serviceCollection,
            string? httpClientName = null)
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                return new NClientFactoryBuilder()
                    .WithCustomHttpClient(httpClientFactory, httpClientName)
                    .WithLogging(loggerFactory)
                    .Build();
            });
        }

        public static IServiceCollection AddNClientFactory(this IServiceCollection serviceCollection,
            Func<INClientFactoryBuilder, INClientFactoryBuilder> configure, string? httpClientName = null)
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var nclientFactoryBuilder = new NClientFactoryBuilder();
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                nclientFactoryBuilder
                    .WithCustomHttpClient(httpClientFactory, httpClientName)
                    .WithLogging(loggerFactory);

                return configure(nclientFactoryBuilder).Build();
            });
        }

        public static IServiceCollection AddNClientFactory(this IServiceCollection serviceCollection,
            Func<IServiceProvider, INClientFactoryBuilder, INClientFactoryBuilder> configure, string? httpClientName = null)
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var nclientFactoryBuilder = new NClientFactoryBuilder();
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                nclientFactoryBuilder
                    .WithCustomHttpClient(httpClientFactory, httpClientName)
                    .WithLogging(loggerFactory);

                return configure(serviceProvider, nclientFactoryBuilder).Build();
            });
        }
    }
}