using System;
using NClient.Abstractions.Exceptions;

namespace NClient.AspNetCore.Exceptions
{
    public class ControllerSideException : NClientException
    {
        public Type ControllerType { get; set; } = null!;

        public ControllerSideException(string message) : base(message)
        {
        }

        public ControllerSideException(string message, Type controllerType) : base(message)
        {
            ControllerType = controllerType;
        }
    }
}