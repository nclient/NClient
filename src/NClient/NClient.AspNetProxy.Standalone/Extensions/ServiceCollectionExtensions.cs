using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Core;
using NClient.Providers.HttpClient.Abstractions;
using NClient.Providers.Resilience.Abstractions;

namespace NClient.AspNetProxy.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection, 
            string host, Func<IClientProviderHttp<TInterface, TController>, IClientProviderLogger<TInterface, TController>> configure)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetService<ILogger<TInterface>>();
                var clientProvider = new AspNetClientProvider().Use<TInterface, TController>(new Uri(host));
                return configure(clientProvider).WithLogger(logger).Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection, 
            string host, IHttpClientProvider httpClientProvider, IResiliencePolicyProvider resiliencePolicyProvider)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetService<ILogger<TInterface>>();
                return new AspNetClientProvider()
                    .Use<TInterface, TController>(new Uri(host))
                    .SetHttpClientProvider(httpClientProvider)
                    .WithResiliencePolicy(resiliencePolicyProvider)
                    .WithLogger(logger)
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
                var logger = serviceProvider.GetService<ILogger<TInterface>>();
                return new AspNetClientProvider()
                    .Use<TInterface, TController>(new Uri(host))
                    .SetHttpClientProvider(httpClientProvider)
                    .WithoutResiliencePolicy()
                    .WithLogger(logger)
                    .Build();
            });
        }
    }
}
