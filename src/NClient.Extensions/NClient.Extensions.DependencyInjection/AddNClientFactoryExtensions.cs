using System;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using Polly;

namespace NClient.Extensions.DependencyInjection
{
    public static class AddNClientFactoryExtensions
    {
        public static IServiceCollection AddNClientFactory(this IServiceCollection serviceCollection,
            JsonSerializerOptions jsonSerializerOptions, IAsyncPolicy<HttpResponse> asyncPolicy, string? httpClientName = null)
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                return new NClientFactoryBuilder()
                    .WithCustomHttpClient(httpClientFactory, httpClientName)
                    .WithCustomSerializer(jsonSerializerOptions)
                    .WithResiliencePolicy(asyncPolicy)
                    .WithLogging(loggerFactory)
                    .Build();
            });
        }

        public static IServiceCollection AddNClientFactory(this IServiceCollection serviceCollection,
            JsonSerializerOptions jsonSerializerOptions, string? httpClientName = null)
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                return new NClientFactoryBuilder()
                    .WithCustomHttpClient(httpClientFactory, httpClientName)
                    .WithCustomSerializer(jsonSerializerOptions)
                    .WithLogging(loggerFactory)
                    .Build();
            });
        }

        public static IServiceCollection AddNClientFactory(this IServiceCollection serviceCollection,
            IAsyncPolicy<HttpResponse> asyncPolicy, string? httpClientName = null)
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                return new NClientFactoryBuilder()
                    .WithCustomHttpClient(httpClientFactory, httpClientName)
                    .WithResiliencePolicy(asyncPolicy)
                    .WithLogging(loggerFactory)
                    .Build();
            });
        }

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
            Func<INClientFactoryBuilder, INClientFactoryBuilder> configure)
        {
            return serviceCollection.AddSingleton(_ => configure(new NClientFactoryBuilder()).Build());
        }

        public static IServiceCollection AddNClientFactory(this IServiceCollection serviceCollection,
            Func<IServiceProvider, INClientFactoryBuilder, INClientFactoryBuilder> configure)
        {
            return serviceCollection.AddSingleton(serviceProvider => configure(serviceProvider, new NClientFactoryBuilder()).Build());
        }
    }
}