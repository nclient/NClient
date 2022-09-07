using System;
using System.Linq;
using NClient.Annotations;
using NClient.Core.Helpers;

namespace NClient.AspNetCore.Controllers
{
    internal static class ControllerQualifier
    {
        public static bool IsNClientController(Type type)
        {
            if (!type.IsClass)
                return false;

            if (!type.IsPublic)
                return false;

            if (type.ContainsGenericParameters)
                return false;

            return type.GetInterfaces().Any() && type.GetInterfaces().Any(IsNClientControllerInterface);
        }

        public static bool IsNClientVirtualController(Type type)
        {
            return type.Assembly.FullName!.StartsWith(NClientAssemblyNames.NClientDynamicControllerProxies);
        }

        public static bool IsNClientControllerInterface(Type type)
        {
            if (!type.IsInterface)
                return false;

            if (!type.IsPublic)
                return false;

            foreach (var declaredInterface in type.GetInterfaces().Concat(new[] { type }))
            {
                if (!type.IsPublic)
                    continue;
                
                if (declaredInterface.IsDefined(typeof(IFacadeAttribute), inherit: true)
                    || declaredInterface.IsDefined(typeof(IPathAttribute), inherit: true)
                    || declaredInterface.GetAllInterfaceMethods(inherit: true).Any(x => x.IsDefined(typeof(IOperationAttribute), inherit: true))
                    || declaredInterface.GetAllInterfaceMethods(inherit: true).SelectMany(x => x.GetParameters()).Any(x => x.IsDefined(typeof(IParamAttribute), inherit: true)))
                    return true;
            }

            return false;
        }
    }
}
