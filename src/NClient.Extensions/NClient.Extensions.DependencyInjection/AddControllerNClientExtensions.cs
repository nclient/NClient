using System;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions;
using NClient.Common.Helpers;
using NClient.Extensions.DependencyInjection.Internals;

namespace NClient.Extensions.DependencyInjection
{
    [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use AddNClient<T> method.")]
    public static class AddControllerNClientExtensions
    {
        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection,
            string host, string? httpClientName = null)
            where TInterface : class
            where TController : TInterface
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));

            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var nclientBuilder = PreBuild<TInterface, TController>(serviceProvider, host, httpClientName);
                return nclientBuilder.Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection,
            string host, Func<IOptionalNClientBuilder<TInterface>, IOptionalNClientBuilder<TInterface>> configure,
            string? httpClientName = null)
            where TInterface : class
            where TController : TInterface
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(configure, nameof(configure));

            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var nclientBuilder = PreBuild<TInterface, TController>(serviceProvider, host, httpClientName);
                return configure(nclientBuilder).Build();
            });
        }

        public static IServiceCollection AddNClient<TInterface, TController>(this IServiceCollection serviceCollection,
            string host, Func<IServiceProvider, IOptionalNClientBuilder<TInterface>, IOptionalNClientBuilder<TInterface>> configure,
            string? httpClientName = null)
            where TInterface : class
            where TController : TInterface
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            Ensure.IsNotNull(configure, nameof(configure));

            return serviceCollection.AddSingleton(serviceProvider =>
            {
                var nclientBuilder = PreBuild<TInterface, TController>(serviceProvider, host, httpClientName);
                return configure(serviceProvider, nclientBuilder).Build();
            });
        }

        private static IOptionalNClientBuilder<TInterface> PreBuild<TInterface, TController>(
            IServiceProvider serviceProvider, string host, string? httpClientName)
            where TInterface : class
            where TController : TInterface
        {
            return new NClientBuilder()
                .Use<TInterface, TController>(host)
                .WithRegisteredProviders(serviceProvider, httpClientName); ;
        }
    }
}
