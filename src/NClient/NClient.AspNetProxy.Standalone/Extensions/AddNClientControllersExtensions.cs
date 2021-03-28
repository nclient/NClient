using System;
using System.Linq;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using NClient.AspNetProxy.Controllers;

namespace NClient.AspNetProxy.Extensions
{
    public static class AddNClientControllersExtensions
    {
        private static readonly IProxyGenerator ProxyGenerator = new ProxyGenerator();
        private static readonly IVirtualControllerRegistrar VirtualControllerRegistrar = new VirtualControllerRegistrar(ProxyGenerator);

        public static IMvcCoreBuilder AddNClientControllers(this IServiceCollection serviceCollection, Action<MvcOptions>? configure = null)
        {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            var mvcCoreBuilder = serviceCollection.AddMvcCore();
            var appAssemblies = mvcCoreBuilder.PartManager.ApplicationParts
                .Where(x => x is AssemblyPart)
                .Cast<AssemblyPart>()
                .Select(x => x.Assembly);
            var appTypes = appAssemblies.SelectMany(x => x.GetTypes());
            var assemblyWithVirtualControllers = VirtualControllerRegistrar.Register(serviceCollection, appTypes);

            var builder = mvcCoreBuilder
                .AddApiExplorer()
                .AddAuthorization()
                .AddCors()
                .AddDataAnnotations()
                .AddFormatterMappings()
                .AddApplicationPart(assemblyWithVirtualControllers)
                .AddControllersAsServices()
                .ConfigureApplicationPartManager(manager =>
                {
                    manager.FeatureProviders.Add(new VirtualControllerFeatureProvider(assemblyWithVirtualControllers));
                });

            if (configure != null)
                builder.AddMvcOptions(configure);

            return builder;
        }
    }
}
