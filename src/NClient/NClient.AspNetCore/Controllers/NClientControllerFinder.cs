using System;
using System.Collections.Generic;
using System.Linq;
using NClient.AspNetCore.Controllers.Models;
using NClient.AspNetCore.Exceptions.Factories;

namespace NClient.AspNetCore.Controllers
{
    internal interface INClientControllerFinder
    {
        IEnumerable<NClientControllerInfo> Find(IEnumerable<Type> types);
    }

    internal class NClientControllerFinder : INClientControllerFinder
    {
        public IEnumerable<NClientControllerInfo> Find(IEnumerable<Type> types)
        {
            var nclientControllers = types.Where(ControllerQualifier.IsNClientController).ToArray();
            if (nclientControllers.Length == 0)
                throw ControllerValidationExceptionFactory.ControllersNotFound();

            var interfaces = new List<Type>();
            foreach (var controller in nclientControllers)
            {
                var nclientInterfaces = controller
                    .GetInterfaces()
                    .Where(ControllerQualifier.IsNClientControllerInterface)
                    .ToArray();
                if (nclientInterfaces.Length == 0)
                    throw ControllerValidationExceptionFactory.ControllerInterfaceNotFound(controller.Name);
                if (nclientInterfaces.Length > 1)
                    throw ControllerValidationExceptionFactory.ControllerCanHaveOnlyOneInterface(controller.Name);

                interfaces.Add(nclientInterfaces.Single());
            }

            return interfaces.Zip(nclientControllers, (@interface, controller) => new NClientControllerInfo(@interface, controller));
        }
    }
}
