using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;

namespace NClient.Extensions.DependencyInjection
{
    [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use AddNClient<T> method.")]
    public static class AddControllerBasedNClientExtensions
    {
        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection,
            string host, string? httpClientName = null)
            where TInterface : class
            where TController : TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();

                return new NClientBuilder()
                    .Use<TInterface, TController>(host)
                    .WithCustomHttpClient(httpClientFactory, httpClientName)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection,
            string host, Func<IControllerBasedClientBuilder<TInterface, TController>, IControllerBasedClientBuilder<TInterface, TController>> configure,
            string? httpClientName = null)
            where TInterface : class
            where TController : TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();

                var nclientBuilder = new NClientBuilder()
                    .Use<TInterface, TController>(host)
                    .WithCustomHttpClient(httpClientFactory, httpClientName)
                    .WithLogging(logger);

                return configure(nclientBuilder).Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection,
            string host, Func<IServiceProvider, IControllerBasedClientBuilder<TInterface, TController>, IControllerBasedClientBuilder<TInterface, TController>> configure,
            string? httpClientName = null)
            where TInterface : class
            where TController : TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();

                var nclientBuilder = new NClientBuilder()
                    .Use<TInterface, TController>(host)
                    .WithCustomHttpClient(httpClientFactory, httpClientName)
                    .WithLogging(logger);

                return configure(serviceProvider, nclientBuilder).Build();
            });
        }
    }
}
