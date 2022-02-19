using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    public static class ConfigureNClientFactoryExtensions
    {
        public static IDiNClientFactoryBuilder<TRequest, TResult> ConfigureNClient<TRequest, TResult>(
            this IDiNClientFactoryBuilder<TRequest, TResult> builder,
            Func<INClientFactoryOptionalBuilder<TRequest, TResult>, INClientFactoryOptionalBuilder<TRequest, TResult>> configure)
        {
            builder.Services.Configure<NClientFactoryBuilderOptions<TRequest, TResult>>(
                builder.Name,
                configureOptions: options => options.BuilderActions.Add(configure));
            return builder;
        }
        
        public static IDiNClientFactoryBuilder<TRequest, TResult> ConfigureNClient<TRequest, TResult>(
            this IDiNClientFactoryBuilder<TRequest, TResult> builder,
            Func<IServiceProvider, INClientFactoryOptionalBuilder<TRequest, TResult>, INClientFactoryOptionalBuilder<TRequest, TResult>> configure)
        {
            builder.Services.AddTransient<IConfigureOptions<NClientFactoryBuilderOptions<TRequest, TResult>>>(services =>
            {
                return new ConfigureNamedOptions<NClientFactoryBuilderOptions<TRequest, TResult>>(builder.Name, (options) =>
                {
                    options.BuilderActions.Add(client => configure(services, client));
                });
            });
            return builder;
        }
    }
}
