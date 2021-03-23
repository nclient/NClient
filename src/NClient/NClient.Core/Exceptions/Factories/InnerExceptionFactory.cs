using System;

namespace NClient.Core.Exceptions.Factories
{
    internal static class InnerExceptionFactory
    {
        public static ArgumentException ArgumentException(string message, string argumentName) =>
            new(message, argumentName);

        public static ArgumentNullException NullArgument(string argumentName) =>
            new(argumentName);

        public static NullReferenceException NullReference(string message) =>
            new(message);

        public static ArgumentException TypeMustBeAttribute(string paramName) => 
            new("Type must be an attribute.", paramName);
    }
}
