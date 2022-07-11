using System;
using NClient.Exceptions;

namespace NClient.AspNetCore.Exceptions
{
    /// <summary>Represents exceptions to return information about an controller-side errors.</summary>
    public class ControllerException : NClientException
    {
        /// <summary>Gets or sets the type of controller.</summary>
        public Type ControllerType { get; set; } = null!;

        /// <summary>Gets or sets the type of controller interface.</summary>
        public Type InterfaceType { get; set; } = null!;

        /// <summary>Initializes a new instance of the exception with a specified error message.</summary>
        /// <param name="message">The message that describes the error.</param>
        public ControllerException(string message) : base(message)
        {
        }

        /// <summary>Initializes a new instance of the exception with a specified error message and controller info.</summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="controllerType">The type of controller.</param>
        /// <param name="interfaceType">The type of controller interface.</param>
        public ControllerException(string message, Type controllerType, Type interfaceType) : base(message)
        {
            ControllerType = controllerType;
            InterfaceType = interfaceType;
        }
    }
}
