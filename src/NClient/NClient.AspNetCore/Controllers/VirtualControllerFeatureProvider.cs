using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace NClient.AspNetCore.Controllers
{
    internal class VirtualControllerFeatureProvider : ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            if (ControllerQualifier.IsNClientController(typeInfo))
                return false;

            if (ControllerQualifier.IsNClientVirtualController(typeInfo))
                return true;

            return IsAspNetNativeController(typeInfo);
        }

        private bool IsAspNetNativeController(TypeInfo typeInfo)
        {
            return base.IsController(typeInfo);
        }
    }
}
