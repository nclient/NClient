using System;
using System.Linq;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using NClient.AspNetCore.Controllers;
using NClient.AspNetCore.Mappers;

namespace NClient.AspNetCore.Extensions
{
    public static class AddNClientControllersExtensions
    {
        private static readonly IProxyGenerator ProxyGenerator = new ProxyGenerator();

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
            var interfaceControllerPairs = new VirtualControllerFinder()
                .FindInterfaceControllerPairs(appTypes);
            var virtualControllerPairs = new VirtualControllerGenerator(new NClientAttributeMapper())
                .Create(interfaceControllerPairs)
                .ToArray();

            foreach (var (virtualControllerType, controllerType) in virtualControllerPairs)
            {
                serviceCollection.AddTransient(controllerType);
                serviceCollection.AddTransient(virtualControllerType, serviceProvider =>
                {
                    var controller = serviceProvider.GetRequiredService(controllerType);
                    return ProxyGenerator.CreateClassProxy(virtualControllerType, new VirtualControllerInterceptor(controller));
                });
            }

            var assemblyWithVirtualControllers = virtualControllerPairs.First().VirtualControllerType.Assembly;
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
                    var defaultControllerFeatureProvider = manager.FeatureProviders
                        .SingleOrDefault(x => x.GetType() == typeof(ControllerFeatureProvider));
                    if (defaultControllerFeatureProvider is not null)
                        manager.FeatureProviders.Remove(defaultControllerFeatureProvider);

                    manager.FeatureProviders.Add(new VirtualControllerFeatureProvider());
                });

            if (configure != null)
                builder.AddMvcOptions(configure);

            return builder;
        }
    }
}
