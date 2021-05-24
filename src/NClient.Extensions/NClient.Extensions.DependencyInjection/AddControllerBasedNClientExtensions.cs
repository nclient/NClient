using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Abstractions.Serialization;

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
                var nclientBuilder = PreBuild<TInterface, TController>(serviceProvider, host, httpClientName);
                return nclientBuilder.Build();
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
                var nclientBuilder = PreBuild<TInterface, TController>(serviceProvider, host, httpClientName);
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
                var nclientBuilder = PreBuild<TInterface, TController>(serviceProvider, host, httpClientName);
                return configure(serviceProvider, nclientBuilder).Build();
            });
        }

        private static IControllerBasedClientBuilder<TInterface, TController> PreBuild<TInterface, TController>(
            IServiceProvider serviceProvider, string host, string? httpClientName)
            where TInterface : class
            where TController : TInterface
        {
            var nclientBuilder = new NClientBuilder()
                .Use<TInterface, TController>(host);

            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            if (httpClientFactory is not null)
                nclientBuilder.WithCustomHttpClient(httpClientFactory, httpClientName);

            var logger = serviceProvider.GetService<ILogger<TInterface>>();
            if (logger is not null)
                nclientBuilder.WithLogging(logger);

            return nclientBuilder;
        }
    }
}
