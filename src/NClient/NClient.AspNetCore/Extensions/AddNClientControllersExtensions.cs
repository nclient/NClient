using System;
using System.Linq;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.AspNetCore.AspNetBinding;
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
                .AddMvcOptions(options =>
                {
                    var serviceProvider = mvcCoreBuilder.Services.BuildServiceProvider();

                    var bodyModelBinderProvider = options.ModelBinderProviders
                        .SingleOrDefault(x => x.GetType() == typeof(Microsoft.AspNetCore.Mvc.ModelBinding.Binders.BodyModelBinderProvider));
                    var bodyModelBinderProviderIndex = options.ModelBinderProviders.IndexOf(bodyModelBinderProvider);
                    options.ModelBinderProviders.Remove(bodyModelBinderProvider);
                    options.ModelBinderProviders.Insert(bodyModelBinderProviderIndex, new BodyModelBinderProvider(
                        options.InputFormatters,
                        serviceProvider.GetRequiredService<IHttpRequestStreamReaderFactory>(),
                        serviceProvider.GetRequiredService<ILoggerFactory>(),
                        options));

                    var complexObjectModelBinderProvider = options.ModelBinderProviders
                        .SingleOrDefault(x => x.GetType().FullName == "Microsoft.AspNetCore.Mvc.ModelBinding.Binders.ComplexObjectModelBinderProvider");
                    var complexTypeModelBinderProvider = options.ModelBinderProviders
                        .SingleOrDefault(x => x.GetType() == typeof(Microsoft.AspNetCore.Mvc.ModelBinding.Binders.ComplexTypeModelBinderProvider));
                    var complexModelBinderProviderIndex = options.ModelBinderProviders.IndexOf(complexObjectModelBinderProvider ?? complexTypeModelBinderProvider);
                    options.ModelBinderProviders.Remove(complexObjectModelBinderProvider ?? complexTypeModelBinderProvider);
                    options.ModelBinderProviders.Insert(complexModelBinderProviderIndex, new ComplexTypeModelBinderProvider());
                })
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
