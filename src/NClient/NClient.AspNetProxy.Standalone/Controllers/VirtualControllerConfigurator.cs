using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NClient.AspNetProxy.Attributes;

namespace NClient.AspNetProxy.Controllers
{
    public class VirtualControllerConfigurator
    {
        private static readonly ProxyGenerator ProxyGeneration = new ProxyGenerator();

        public IServiceCollection ServiceCollection { get; }
        public IList<(Type InterfaceType, Type ControllerType)> InterfaceControllerPairs { get; }

        public VirtualControllerConfigurator(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
            InterfaceControllerPairs = new List<(Type, Type)>();
        }

        public VirtualControllerConfigurator AddNClientController<TInterface, TController>()
            where TController : ControllerBase, TInterface
        {
            InterfaceControllerPairs.Add((typeof(TInterface), typeof(TController)));
            return this;
        }

        internal void ApplyChanges()
        {
            if (InterfaceControllerPairs.Count == 0)
                return;

            var virtualControllerGenerator = new VirtualControllerGenerator(new NClientAttributeMapper());
            var virtualControllerPairs = virtualControllerGenerator
                .Create(InterfaceControllerPairs)
                .ToArray();
            foreach (var (virtualControllerType, controllerType) in virtualControllerPairs)
            {
                ServiceCollection.AddTransient(controllerType);
                ServiceCollection.AddTransient(virtualControllerType, serviceProvider =>
                {
                    var controller = serviceProvider.GetRequiredService(controllerType);
                    return ProxyGeneration.CreateClassProxy(virtualControllerType, new VirtualControllerInterceptor(controller));
                });
            }

            var assemblyWithVirtualControllers = virtualControllerPairs.First().VirtualControllerType.Assembly;
            ServiceCollection
                .AddMvcCore()
                .AddControllersAsServices()
                .AddApplicationPart(assemblyWithVirtualControllers)
                .ConfigureApplicationPartManager(manager =>
                {
                    manager.FeatureProviders.Add(new VirtualControllerFeatureProvider(assemblyWithVirtualControllers));
                });
        }
    }
}
