using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.AspNetProxy.Exceptions.Factories;
using NClient.AspNetProxy.Mappers;

namespace NClient.AspNetProxy.Controllers
{
    public interface IVirtualControllerRegistrar
    {
        Assembly Register(IServiceCollection serviceCollection, IEnumerable<Type> appTypes);
    }

    public class VirtualControllerRegistrar : IVirtualControllerRegistrar
    {
        private const string ControllerSuffix = "Controller";
        private const string ClientSuffix = "Client";

        private readonly IProxyGenerator _proxyGenerator;

        public VirtualControllerRegistrar(IProxyGenerator proxyGenerator)
        {
            _proxyGenerator = proxyGenerator;
        }

        public Assembly Register(IServiceCollection serviceCollection, IEnumerable<Type> appTypes)
        {
            var interfaceControllerPairs = FindInterfaceControllerPairs(appTypes);

            var virtualControllerGenerator = new VirtualControllerGenerator(new NClientAttributeMapper());
            var virtualControllerPairs = virtualControllerGenerator
                .Create(interfaceControllerPairs)
                .ToArray();
            foreach (var (virtualControllerType, controllerType) in virtualControllerPairs)
            {
                serviceCollection.AddTransient(controllerType);
                serviceCollection.AddTransient(virtualControllerType, serviceProvider =>
                {
                    var controller = serviceProvider.GetRequiredService(controllerType);
                    return _proxyGenerator.CreateClassProxy(virtualControllerType, new VirtualControllerInterceptor(controller));
                });
            }
            return virtualControllerPairs.First().VirtualControllerType.Assembly;
        }

        private static IEnumerable<(Type InterfaceType, Type ControllerType)> FindInterfaceControllerPairs(IEnumerable<Type> types)
        {
            var controllers = types.Where(IsNClientController).ToArray();
            var interfaces = new List<Type>();
            foreach (var controller in controllers)
            {
                var nclientInterfaces = controller.GetInterfaces().Where(IsNClientControllerInterface).ToArray();
                if (nclientInterfaces.Length == 0)
                    throw OuterAspNetExceptionFactory.ControllerInterfaceNotFound(controller.Name);
                if (nclientInterfaces.Length > 1)
                    throw OuterAspNetExceptionFactory.ControllerCanHaveOnlyOneInterface(controller.Name);

                interfaces.Add(nclientInterfaces.Single());
            }

            return interfaces.Zip(controllers, (@interface, controller) => (@interface, controller));
        }

        private static bool IsNClientController(Type type)
        {
            if (!type.IsClass)
                return false;

            if (type.IsAbstract)
                return false;

            if (!type.IsPublic)
                return false;

            if (type.ContainsGenericParameters)
                return false;

            if (type.IsDefined(typeof(NonControllerAttribute), inherit: false))
                return false;

            if (!type.Name.EndsWith(ControllerSuffix, StringComparison.OrdinalIgnoreCase))
                return false;

            if (type.IsDefined(typeof(ControllerAttribute), inherit: false))
                return false;

            if (!type.GetInterfaces().Any(IsNClientControllerInterface))
                return false;

            return true;
        }

        private static bool IsNClientControllerInterface(Type type)
        {
            if (type.IsClass)
                return false;

            if (!type.IsPublic)
                return false;

            if (type.ContainsGenericParameters)
                return false;

            if (!type.Name.EndsWith(ControllerSuffix, StringComparison.OrdinalIgnoreCase)
                && !type.Name.EndsWith(ClientSuffix, StringComparison.OrdinalIgnoreCase))
                return false;

            if (!type.IsDefined(typeof(PathAttribute), inherit: true)
                && !type.GetMethods().Any(x => x.IsDefined(typeof(MethodAttribute), inherit: true)))
                return false;

            return true;
        }
    }
}
