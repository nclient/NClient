using System;
using System.Linq;
using System.Reflection;
using NClient.Annotations;
using NClient.Annotations.Methods;

namespace NClient.AspNetCore.Controllers
{
    public static class ControllerQualifier
    {
        private const string ControllerSuffix = "Controller";
        private const string ClientSuffix = "Client";

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
            return type.Assembly.FullName.StartsWith(NClientAssemblyNames.NClientDynamicControllerProxies);
        }

        public static bool IsNClientControllerInterface(Type type)
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
