using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.InterfaceBasedClients;
using Polly;

namespace NClient.Extensions.DependencyInjection
{
    public static class InterfaceBasedClientExtensions
    {
        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection,
            string host, IAsyncPolicy<HttpResponse> asyncPolicy)
            where TInterface : class
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

                return new NClientBuilder()
                    .Use<TInterface>(host, httpClientFactory)
                    .WithResiliencePolicy(asyncPolicy)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection,
            string host)
            where TInterface : class
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

                return new NClientBuilder()
                    .Use<TInterface>(host, httpClientFactory)
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
            string host, IHttpClientProvider httpClientProvider, IResiliencePolicyProvider resiliencePolicyProvider)
            where TInterface : class
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                return new NClientBuilder()
                    .Use<TInterface>(host, httpClientProvider)
                    .WithResiliencePolicy(resiliencePolicyProvider)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection,
            string host, IHttpClientProvider httpClientProvider)
            where TInterface : class
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                return new NClientBuilder()
                    .Use<TInterface>(host, httpClientProvider)
                    .WithLogging(logger)
                    .Build();
            });
        }
    }
}
