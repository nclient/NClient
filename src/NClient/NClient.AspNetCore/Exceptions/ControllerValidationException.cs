using System;

namespace NClient.AspNetCore.Exceptions
{
    /// <summary>
    /// Represents exceptions to return information about an invalid controller.
    /// </summary>
    public class ControllerValidationException : ControllerException
    {
        public ControllerValidationException(string message) : base(message)
        {
        }

        public ControllerValidationException(string message, Type controllerType, Type interfaceType)
            : base(message, controllerType, interfaceType)
        {
        }
    }
}