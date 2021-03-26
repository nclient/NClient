using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Clients;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
#pragma warning disable 618

namespace NClient.AspNetProxy.Extensions
{
    public static class AddNClientExtensions
    {
        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection, 
            Func<INClientControllerProvider, INClientControllerProvider<TInterface, TController>> configure)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            return serviceCollection.AddSingleton(_ => configure(new NClientControllerProvider()).Build());
        }

        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection, 
            string host, IHttpClientProvider httpClientProvider, IResiliencePolicyProvider resiliencePolicyProvider)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                return new NClientControllerProvider()
                    .Use<TInterface, TController>(host, httpClientProvider)
                    .WithResiliencePolicy(resiliencePolicyProvider)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection, 
            string host, IHttpClientProvider httpClientProvider)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                return new NClientControllerProvider()
                    .Use<TInterface, TController>(host, httpClientProvider)
                    .WithLogging(logger)
                    .Build();
            });
        }
    }
}
