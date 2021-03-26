using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Clients;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;

namespace NClient.AspNetProxy.Extensions
{
    public static class AddNClientExtensions
    {
        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection, 
            string host, Func<IControllerClientProviderHttp<TInterface, TController>, IControllerClientProviderLogger<TInterface, TController>> configure)
            where TInterface : class, INClient
            where TController : ControllerBase, TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                var clientProvider = new ControllerClientProvider().Use<TInterface, TController>(new Uri(host));
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
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                return new ControllerClientProvider()
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
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                return new ControllerClientProvider()
                    .Use<TInterface, TController>(new Uri(host))
                    .SetHttpClientProvider(httpClientProvider)
                    .WithoutResiliencePolicy()
                    .WithLogger(logger)
                    .Build();
            });
        }
    }
}
