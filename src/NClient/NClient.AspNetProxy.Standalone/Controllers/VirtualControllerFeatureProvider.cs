using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace NClient.AspNetProxy.Controllers
{
    internal class VirtualControllerFeatureProvider : ControllerFeatureProvider
    {
        private readonly Assembly _virtualControllerAssembly;
        private readonly HashSet<string> _virtualControllerNames;

        public VirtualControllerFeatureProvider(Assembly virtualControllerAssembly)
        {
            _virtualControllerAssembly = virtualControllerAssembly;
            _virtualControllerNames = new HashSet<string>(_virtualControllerAssembly.GetTypes().Select(x => x.Name));
        }

        protected override bool IsController(TypeInfo typeInfo)
        {
            if (typeInfo.Assembly == _virtualControllerAssembly)
                return true;

            var baseResult = base.IsController(typeInfo);

            if (baseResult == false)
                return false;

            if (_virtualControllerNames.Contains(typeInfo.Name))
                return false;

            return baseResult;
        }
    }
}
