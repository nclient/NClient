using System;

namespace NClient.AspNetCore.Controllers.Models
{
    public class NClientControllerInfo
    {
        public Type InterfaceType { get; }
        public Type ControllerType { get; }

        public NClientControllerInfo(Type interfaceType, Type controllerType)
        {
            InterfaceType = interfaceType;
            ControllerType = controllerType;
        }
    }
}