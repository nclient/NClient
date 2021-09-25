using System;

namespace NClient.AspNetCore.Exceptions
{
    public class ControllerArgumentException : ControllerException
    {
        public ControllerArgumentException(string message) : base(message)
        {
        }

        public ControllerArgumentException(string message, Type controllerType, Type interfaceType) : base(message, controllerType, interfaceType)
        {
        }
    }
}
