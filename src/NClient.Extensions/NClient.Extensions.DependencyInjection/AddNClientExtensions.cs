using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Clients;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.InterfaceProxy;
using NClient.InterfaceProxy.Extensions;
using NClient.Providers.HttpClient.RestSharp;
using RestSharp.Authenticators;
using Polly;

namespace NClient.Extensions.DependencyInjection
{
    public static class AddNClientExtensions
    {
        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection,
            string host, IAuthenticator authenticator, IAsyncPolicy asyncPolicy)
            where TInterface : class, INClient
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                return new ClientProvider()
                    .Use<TInterface>(host, authenticator)
                    .WithResiliencePolicy(asyncPolicy)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection,
            string host, IAuthenticator authenticator)
            where TInterface : class, INClient
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                return new ClientProvider()
                    .Use<TInterface>(host, authenticator)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection, 
            string host, IAsyncPolicy asyncPolicy)
            where TInterface : class, INClient
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                return new ClientProvider()
                    .Use<TInterface>(host)
                    .WithResiliencePolicy(asyncPolicy)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection, 
            string host)
            where TInterface : class, INClient
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                return new ClientProvider()
                    .Use<TInterface>(host, new RestSharpHttpClientProvider())
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection, 
            Func<IClientProvider, IClientProvider<TInterface>> configure) 
            where TInterface : class, INClient
        {
            return serviceCollection.AddSingleton(_ => configure(new ClientProvider()).Build());
        }

        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection, 
            string host, IHttpClientProvider httpClientProvider, IResiliencePolicyProvider resiliencePolicyProvider)
            where TInterface : class, INClient
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                return new ClientProvider()
                    .Use<TInterface>(host, httpClientProvider)
                    .WithResiliencePolicy(resiliencePolicyProvider)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection, 
            string host, IHttpClientProvider httpClientProvider)
            where TInterface : class, INClient
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                return new ClientProvider()
                    .Use<TInterface>(host, httpClientProvider)
                    .WithLogging(logger)
                    .Build();
            });
        }
    }
}
