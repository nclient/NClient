using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NClient.Core.Helpers
{
    internal static class MethodInfoExtensions
    {
        public static bool isVirtual(this MethodInfo methodInfo)
        {
            return ((methodInfo.Attributes & MethodAttributes.Virtual) != 0);
        }
        public static bool isNewSlot(this MethodInfo methodInfo)
        {
            return ((methodInfo.Attributes & MethodAttributes.NewSlot) != 0);
        }

        public static MethodInfo GetShadowOrBase(this MethodInfo method)
        {
            System.Diagnostics.Debug.WriteLine($" ShadowOrBase: { method.DeclaringType.Name } - {method.GetBaseDefinition()}");
            return method.GetBaseDefinition();
        }

        public static bool shadowsByName(this MethodInfo method)
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            if (method.DeclaringType.BaseType.GetMethods(flags).Any(m => m.Name == method.Name))
            {
                //method shadows by name
                return true;
            }
            return false;
        }

    }
}
