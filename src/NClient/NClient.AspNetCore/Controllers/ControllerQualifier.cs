using System;
using System.Linq;
using NClient.Annotations;
using NClient.Annotations.Operations;
using NClient.Annotations.Parameters;

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

            if (type.ContainsGenericParameters)
                return false;

            return type.IsDefined(typeof(FacadeAttribute), inherit: true)
                || type.IsDefined(typeof(ApiAttribute), inherit: true)
                || type.IsDefined(typeof(PathAttribute), inherit: true)
                || type.GetMethods().Any(x => x.IsDefined(typeof(OperationAttribute), inherit: true))
                || type.GetMethods().SelectMany(x => x.GetParameters()).Any(x => x.IsDefined(typeof(ParamAttribute), inherit: true));
        }
    }
}
