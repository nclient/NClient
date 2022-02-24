using System;

namespace NClient.AspNetCore.Exceptions
{
    /// <summary>Represents exceptions thrown due to invalid arguments passed to a controller methods.</summary>
    public class ControllerArgumentException : ControllerException
    {
        /// <summary>Initializes a new instance of the exception with a specified error message.</summary>
        /// <param name="message">The message that describes the error.</param>
        public ControllerArgumentException(string message) : base(message)
        {
        }

        /// <summary>Initializes a new instance of the exception with a specified error message and controller info.</summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="controllerType">The type of controller.</param>
        /// <param name="interfaceType">The type of controller interface.</param>
        public ControllerArgumentException(string message, Type controllerType, Type interfaceType) 
            : base(message, controllerType, interfaceType)
        {
        }
    }
}
