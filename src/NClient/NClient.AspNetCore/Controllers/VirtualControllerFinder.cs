using System;
using System.Collections.Generic;
using System.Linq;
using NClient.AspNetCore.Exceptions.Factories;

namespace NClient.AspNetCore.Controllers
{
    internal interface IVirtualControllerFinder
    {
        IEnumerable<(Type InterfaceType, Type ControllerType)> FindInterfaceControllerPairs(IEnumerable<Type> types);
    }

    internal class VirtualControllerFinder : IVirtualControllerFinder
    {
        public IEnumerable<(Type InterfaceType, Type ControllerType)> FindInterfaceControllerPairs(IEnumerable<Type> types)
        {
            var nclientControllers = types.Where(ControllerQualifier.IsNClientController).ToArray();
            if (nclientControllers.Length == 0)
                throw OuterAspNetExceptionFactory.ControllersNotFound();

            var interfaces = new List<Type>();
            foreach (var controller in nclientControllers)
            {
                var nclientInterfaces = controller
                    .GetInterfaces()
                    .Where(ControllerQualifier.IsNClientControllerInterface)
                    .ToArray();
                if (nclientInterfaces.Length == 0)
                    throw OuterAspNetExceptionFactory.ControllerInterfaceNotFound(controller.Name);
                if (nclientInterfaces.Length > 1)
                    throw OuterAspNetExceptionFactory.ControllerCanHaveOnlyOneInterface(controller.Name);

                interfaces.Add(nclientInterfaces.Single());
            }

            return interfaces.Zip(nclientControllers, (@interface, controller) => (@interface, controller));
        }
    }
}
