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
    [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use AddNClient<T> method.")]
    public static class AddControllerBasedNClientExtensions
    {
        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection,
            string host, JsonSerializerOptions jsonSerializerOptions, IAsyncPolicy<HttpResponse> asyncPolicy, string? httpClientName = null)
            where TInterface : class
            where TController : TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

                return new NClientBuilder()
                    .Use<TInterface, TController>(host)
                    .WithCustomHttpClient(httpClientFactory, httpClientName)
                    .WithCustomSerializer(jsonSerializerOptions)
                    .WithResiliencePolicy(asyncPolicy)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection,
            string host, JsonSerializerOptions jsonSerializerOptions, string? httpClientName = null)
            where TInterface : class
            where TController : TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

                return new NClientBuilder()
                    .Use<TInterface, TController>(host)
                    .WithCustomHttpClient(httpClientFactory, httpClientName)
                    .WithCustomSerializer(jsonSerializerOptions)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection,
            string host, IAsyncPolicy<HttpResponse> asyncPolicy, string? httpClientName = null)
            where TInterface : class
            where TController : TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

                return new NClientBuilder()
                    .Use<TInterface, TController>(host)
                    .WithCustomHttpClient(httpClientFactory, httpClientName)
                    .WithResiliencePolicy(asyncPolicy)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection,
            string host, string? httpClientName = null)
            where TInterface : class
            where TController : TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

                return new NClientBuilder()
                    .Use<TInterface, TController>(host)
                    .WithCustomHttpClient(httpClientFactory, httpClientName)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection,
            Func<INClientBuilder, IControllerBasedClientBuilder<TInterface, TController>> configure)
            where TInterface : class
            where TController : TInterface
        {
            return serviceCollection.AddSingleton(_ => configure(new NClientBuilder()).Build());
        }

        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection,
            Func<IServiceProvider, INClientBuilder, IControllerBasedClientBuilder<TInterface, TController>> configure)
            where TInterface : class
            where TController : TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider => configure(serviceProvider, new NClientBuilder()).Build());
        }
    }
}
