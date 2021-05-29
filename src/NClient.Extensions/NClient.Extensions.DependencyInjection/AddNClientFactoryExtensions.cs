using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;
using NClient.Common.Helpers;

namespace NClient.Extensions.DependencyInjection
{
    public static class AddNClientFactoryExtensions
    {
        public static IServiceCollection AddNClientFactory(this IServiceCollection serviceCollection,
            string? httpClientName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));

            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var nclientFactoryBuilder = PreBuild(serviceProvider, httpClientName);
                return nclientFactoryBuilder.Build();
            });
        }

        public static IServiceCollection AddNClientFactory(this IServiceCollection serviceCollection,
            Func<IOptionalNClientFactoryBuilder, IOptionalNClientFactoryBuilder> configure, string? httpClientName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(configure, nameof(configure));

            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var nclientFactoryBuilder = PreBuild(serviceProvider, httpClientName);
                return configure(nclientFactoryBuilder).Build();
            });
        }

        public static IServiceCollection AddNClientFactory(this IServiceCollection serviceCollection,
            Func<IServiceProvider, IOptionalNClientFactoryBuilder, IOptionalNClientFactoryBuilder> configure, string? httpClientName = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(configure, nameof(configure));

            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var nclientFactoryBuilder = PreBuild(serviceProvider, httpClientName);
                return configure(serviceProvider, nclientFactoryBuilder).Build();
            });
        }

        private static IOptionalNClientFactoryBuilder PreBuild(
            IServiceProvider serviceProvider, string? httpClientName)
        {
            var nclientFactoryBuilder = new NClientFactoryBuilder();

            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            if (httpClientFactory is not null)
                nclientFactoryBuilder.WithCustomHttpClient(httpClientFactory, httpClientName);

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            if (loggerFactory is not null)
                nclientFactoryBuilder.WithLogging(loggerFactory);

            return nclientFactoryBuilder;
        }
    }
}