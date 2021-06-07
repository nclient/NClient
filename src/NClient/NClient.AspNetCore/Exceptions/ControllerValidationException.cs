using System;

namespace NClient.AspNetCore.Exceptions
{
    public class ControllerValidationException : ControllerSideException
    {
        public ControllerValidationException(string message) : base(message)
        {
        }

        public ControllerValidationException(string message, Type controllerType) : base(message, controllerType)
        {
        }
    }
}