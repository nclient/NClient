using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    public static class ConfigureNClientExtensions
    {
        public static IDiNClientBuilder<TClient, TRequest, TResult> ConfigureNClient<TClient, TRequest, TResult>(
            this IDiNClientBuilder<TClient, TRequest, TResult> builder,
            Func<INClientOptionalBuilder<TClient, TRequest, TResult>, INClientOptionalBuilder<TClient, TRequest, TResult>> configure)
            where TClient : class
        {
            builder.Services.Configure<NClientBuilderOptions<TClient, TRequest, TResult>>(
                builder.Name,
                configureOptions: options => options.BuilderActions.Add(configure));
            return builder;
        }
        
        public static IDiNClientBuilder<TClient, TRequest, TResult> ConfigureNClient<TClient, TRequest, TResult>(
            this IDiNClientBuilder<TClient, TRequest, TResult> builder,
            Func<IServiceProvider, INClientOptionalBuilder<TClient, TRequest, TResult>, INClientOptionalBuilder<TClient, TRequest, TResult>> configure)
            where TClient : class
        {
            builder.Services.AddTransient<IConfigureOptions<NClientBuilderOptions<TClient, TRequest, TResult>>>(services =>
            {
                return new ConfigureNamedOptions<NClientBuilderOptions<TClient, TRequest, TResult>>(builder.Name, (options) =>
                {
                    options.BuilderActions.Add(client => configure(services, client));
                });
            });
            return builder;
        }
    }
}
