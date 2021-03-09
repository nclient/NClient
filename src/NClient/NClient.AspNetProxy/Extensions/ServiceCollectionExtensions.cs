using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Core;
using Polly;

namespace NClient.AspNetProxy.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection, 
            string host, IAsyncPolicy asyncPolicy)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetService<ILogger<TInterface>>();
                return new AspNetClientProvider()
                    .Use<TInterface, TController>(new Uri(host))
                    .SetDefaultHttpClientProvider()
                    .WithPollyResiliencePolicy(asyncPolicy)
                    .WithLogger(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection, 
            string host)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetService<ILogger<TInterface>>();
                return new AspNetClientProvider()
                    .Use<TInterface, TController>(new Uri(host))
                    .SetDefaultHttpClientProvider()
                    .WithoutResiliencePolicy()
                    .WithLogger(logger)
                    .Build();
            });
        }
    }
}
