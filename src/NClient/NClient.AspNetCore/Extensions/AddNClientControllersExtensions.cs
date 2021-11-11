using System;
using System.Collections.Generic;
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
using NClient.AspNetCore.Controllers.Models;
using NClient.AspNetCore.Exceptions.Factories;
using NClient.AspNetCore.Filters;
using NClient.AspNetCore.Mappers;
using NClient.Common.Helpers;
using NClient.Core.Helpers;
using NClient.Core.Helpers.ObjectMemberManagers;

#pragma warning disable 618

namespace NClient.AspNetCore.Extensions
{
    public static class AddNClientControllersExtensions
    {
        private static readonly IProxyGenerator ProxyGenerator;
        private static readonly INClientControllerFinder NClientControllerFinder;
        private static readonly IVirtualControllerGenerator VirtualControllerGenerator;

        static AddNClientControllersExtensions()
        {
            ProxyGenerator = new ProxyGenerator();
            NClientControllerFinder = new NClientControllerFinder(new ControllerValidationExceptionFactory());
            VirtualControllerGenerator = new VirtualControllerGenerator(
                new VirtualControllerAttributeBuilder(
                    new ObjectMemberManager(new ControllerObjectMemberManagerExceptionFactory()),
                    new ControllerValidationExceptionFactory()),
                new NClientAttributeMapper(),
                new ControllerValidationExceptionFactory(),
                new GuidProvider());
        }

        /// <summary>
        /// Adds a NClient controllers to the DI container.
        /// </summary>
        public static IMvcCoreBuilder AddNClientControllers(this IServiceCollection serviceCollection,
            Action<MvcOptions>? configure = null)
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));

            var mvcCoreBuilder = serviceCollection.AddMvcCore();
            var appAssemblies = mvcCoreBuilder.PartManager.ApplicationParts
                .Where(x => x is AssemblyPart)
                .Cast<AssemblyPart>()
                .Select(x => x.Assembly);
            var appTypes = appAssemblies.SelectMany(x => x.GetTypes());
            var virtualControllers = GetVirtualControllers(appTypes);

            foreach (var virtualController in virtualControllers)
            {
                serviceCollection.AddTransient(virtualController.ControllerType);
                serviceCollection.AddTransient(virtualController.Type, serviceProvider =>
                {
                    var controller = serviceProvider.GetRequiredService(virtualController.ControllerType);
                    return ProxyGenerator.CreateClassProxy(
                        virtualController.Type,
                        new VirtualControllerInterceptor(controller));
                });
            }

            var assemblyWithVirtualControllers = virtualControllers.First().Type.Assembly;
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

        /// <summary>
        /// Registers the NClient exception filter to the DI container.
        /// </summary>
        public static IMvcCoreBuilder WithResponseExceptions(this IMvcCoreBuilder builder)
        {
            Ensure.IsNotNull(builder, nameof(builder));

            builder.Services.Configure<MvcOptions>(x => x.Filters.Add(new HttpResponseExceptionFilter()));
            return builder;
        }

        private static VirtualControllerInfo[] GetVirtualControllers(IEnumerable<Type> appTypes)
        {
            var nclientControllers = NClientControllerFinder.Find(appTypes);
            return VirtualControllerGenerator
                .Create(nclientControllers)
                .ToArray();
        }
    }
}
