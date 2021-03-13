using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Core;
using NClient.InterfaceProxy;
using NClient.InterfaceProxy.Extensions;
using NClient.Providers.HttpClient.Abstractions;
using NClient.Providers.Resilience.Abstractions;
using Polly;

namespace NClient.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection, 
            string host, IAsyncPolicy asyncPolicy)
            where TInterface : class, INClient
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                return new ClientProvider()
                    .Use<TInterface>(new Uri(host))
                    .SetDefaultHttpClientProvider()
                    .WithPollyResiliencePolicy(asyncPolicy)
                    .WithLogger(logger)
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
                    .Use<TInterface>(new Uri(host))
                    .SetDefaultHttpClientProvider()
                    .WithoutResiliencePolicy()
                    .WithLogger(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection, 
            string host, Func<IClientProviderHttp<TInterface>, IClientProviderLogger<TInterface>> configure) 
            where TInterface : class, INClient
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                var clientProvider = new ClientProvider().Use<TInterface>(new Uri(host));
                return configure(clientProvider).WithLogger(logger).Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface>(this IServiceCollection serviceCollection, 
            string host, IHttpClientProvider httpClientProvider, IResiliencePolicyProvider resiliencePolicyProvider)
            where TInterface : class, INClient
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                return new ClientProvider()
                    .Use<TInterface>(new Uri(host))
                    .SetHttpClientProvider(httpClientProvider)
                    .WithResiliencePolicy(resiliencePolicyProvider)
                    .WithLogger(logger)
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
                    .Use<TInterface>(new Uri(host))
                    .SetHttpClientProvider(httpClientProvider)
                    .WithoutResiliencePolicy()
                    .WithLogger(logger)
                    .Build();
            });
        }
    }
}
