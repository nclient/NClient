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
    public static class AddInterfaceBasedNClientExtensions
    {
        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection,
            string host, JsonSerializerOptions jsonSerializerOptions, IAsyncPolicy<HttpResponse> asyncPolicy, string? httpClientName = null)
            where TInterface : class
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

                return new NClientBuilder()
                    .Use<TInterface>(host)
                    .WithCustomHttpClient(httpClientFactory, httpClientName)
                    .WithCustomSerializer(jsonSerializerOptions)
                    .WithResiliencePolicy(asyncPolicy)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection,
            string host, JsonSerializerOptions jsonSerializerOptions, string? httpClientName = null)
            where TInterface : class
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

                return new NClientBuilder()
                    .Use<TInterface>(host)
                    .WithCustomHttpClient(httpClientFactory, httpClientName)
                    .WithCustomSerializer(jsonSerializerOptions)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection,
            string host, IAsyncPolicy<HttpResponse> asyncPolicy, string? httpClientName = null)
            where TInterface : class
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

                return new NClientBuilder()
                    .Use<TInterface>(host)
                    .WithCustomHttpClient(httpClientFactory, httpClientName)
                    .WithResiliencePolicy(asyncPolicy)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection,
            string host, string? httpClientName = null)
            where TInterface : class
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

                return new NClientBuilder()
                    .Use<TInterface>(host)
                    .WithCustomHttpClient(httpClientFactory, httpClientName)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection,
            Func<INClientBuilder, IInterfaceBasedClientBuilder<TInterface>> configure)
            where TInterface : class
        {
            return serviceCollection.AddSingleton(_ => configure(new NClientBuilder()).Build());
        }

        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection,
            Func<IServiceProvider, INClientBuilder, IInterfaceBasedClientBuilder<TInterface>> configure)
            where TInterface : class
        {
            return serviceCollection.AddSingleton(serviceProvider => configure(serviceProvider, new NClientBuilder()).Build());
        }
    }
}
