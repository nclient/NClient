using System;

namespace NClient.AspNetCore.Exceptions
{
    /// <summary>Represents exceptions to return information about an invalid controller.</summary>
    public class ControllerValidationException : ControllerException
    {
        /// <summary>Initializes a new instance of the exception with a specified error message.</summary>
        /// <param name="message">The message that describes the error.</param>
        public ControllerValidationException(string message) : base(message)
        {
        }

        /// <summary>Initializes a new instance of the exception with a specified error message and controller info.</summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="controllerType">The type of controller.</param>
        /// <param name="interfaceType">The type of controller interface.</param>
        public ControllerValidationException(string message, Type controllerType, Type interfaceType)
            : base(message, controllerType, interfaceType)
        {
        }
    }
}
