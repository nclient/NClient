using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Standalone;
using NClient.Standalone.ControllerBasedClients;
using RestSharp.Authenticators;
using Polly;

namespace NClient.Extensions.DependencyInjection
{
    [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use AddNClient<T> method.")]
    public static class ControllerBasedClientExtensions
    {
        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection,
            string host, IAuthenticator authenticator, IAsyncPolicy asyncPolicy)
            where TInterface : class
            where TController : TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                return new NClientBuilder()
                    .Use<TInterface, TController>(host, authenticator)
                    .WithResiliencePolicy(asyncPolicy)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection,
            string host, IAuthenticator authenticator)
            where TInterface : class
            where TController : TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                return new NClientBuilder()
                    .Use<TInterface, TController>(host, authenticator)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection,
            string host, IAsyncPolicy asyncPolicy)
            where TInterface : class
            where TController : TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                return new NClientBuilder()
                    .Use<TInterface, TController>(host)
                    .WithResiliencePolicy(asyncPolicy)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection,
            string host)
            where TInterface : class
            where TController : TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                return new NClientBuilder()
                    .Use<TInterface, TController>(host)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection,
            Func<INClientBuilder, IControllerBasedClientBuilder<TInterface, TController>> configure)
            where TInterface : class
            where TController : TInterface
        {
            return serviceCollection.AddSingleton(_ => configure(new NClientBuilder()).Build());
        }

        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection,
            string host, IHttpClientProvider httpClientProvider, IResiliencePolicyProvider resiliencePolicyProvider)
            where TInterface : class
            where TController : TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                return new NClientBuilder()
                    .Use<TInterface, TController>(host, httpClientProvider)
                    .WithResiliencePolicy(resiliencePolicyProvider)
                    .WithLogging(logger)
                    .Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection,
            string host, IHttpClientProvider httpClientProvider)
            where TInterface : class
            where TController : TInterface
        {
            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TInterface>>();
                return new NClientBuilder()
                    .Use<TInterface, TController>(host, httpClientProvider)
                    .WithLogging(logger)
                    .Build();
            });
        }
    }
}
