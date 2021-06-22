using System;
using NClient.Abstractions.Exceptions;

namespace NClient.AspNetCore.Exceptions
{
    /// <summary>
    /// Represents exceptions to return information about an controller-side errors.
    /// </summary>
    public class ControllerException : NClientException
    {
        /// <summary>
        /// The type of controller.
        /// </summary>
        public Type ControllerType { get; set; } = null!;

        /// <summary>
        /// The type of controller interface.
        /// </summary>
        public Type InterfaceType { get; set; } = null!;

        public ControllerException(string message) : base(message)
        {
        }

        public ControllerException(string message, Type controllerType, Type interfaceType) : base(message)
        {
            ControllerType = controllerType;
            InterfaceType = interfaceType;
        }
    }
}