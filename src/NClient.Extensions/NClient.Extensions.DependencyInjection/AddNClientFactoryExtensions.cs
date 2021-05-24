using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions;

namespace NClient.Extensions.DependencyInjection
{
    public static class AddNClientFactoryExtensions
    {
        public static IServiceCollection AddNClientFactory(this IServiceCollection serviceCollection,
            string? httpClientName = null)
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var nclientFactoryBuilder = PreBuild(serviceProvider, httpClientName);
                return nclientFactoryBuilder.Build();
            });
        }

        public static IServiceCollection AddNClientFactory(this IServiceCollection serviceCollection,
            Func<INClientFactoryBuilder, INClientFactoryBuilder> configure, string? httpClientName = null)
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var nclientFactoryBuilder = PreBuild(serviceProvider, httpClientName);
                return configure(nclientFactoryBuilder).Build();
            });
        }

        public static IServiceCollection AddNClientFactory(this IServiceCollection serviceCollection,
            Func<IServiceProvider, INClientFactoryBuilder, INClientFactoryBuilder> configure, string? httpClientName = null)
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var nclientFactoryBuilder = PreBuild(serviceProvider, httpClientName);
                return configure(serviceProvider, nclientFactoryBuilder).Build();
            });
        }
        
        private static INClientFactoryBuilder PreBuild(
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