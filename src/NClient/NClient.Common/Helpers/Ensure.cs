using System;
using System.Diagnostics;

namespace NClient.Common.Helpers
{
    [DebuggerStepThrough]
    internal static class Ensure
    {
        public static T IsNotNull<T>(T? value, string paramName)
        {
            if (value == null)
                throw EnsureExceptionFactory.CreateArgumentNullException(paramName);
            return value;
        }

        public static string IsNotNullOrEmpty(string value, string paramName)
        {
            if (value == null)
                throw EnsureExceptionFactory.CreateArgumentNullException(paramName);

            if (value.Length == 0)
                throw EnsureExceptionFactory.CreateEmptyArgumentException(paramName);

            return value;
        }

        public static T IsCompatibleWith<T>(object value, string paramName) where T : class
        {
            if (value is not T)
                throw EnsureExceptionFactory.CreateIncompatibleArgumentException<T>(paramName);
            return (T)value;
        }
    }

    internal static class EnsureExceptionFactory
    {
        public static ArgumentNullException CreateArgumentNullException(string paramName) =>
            new(paramName, "Value cannot be null.");

        public static ArgumentException CreateEmptyArgumentException(string paramName) =>
            new("Value cannot be empty.", paramName);

        public static ArgumentException CreateIncompatibleArgumentException<T>(string paramName) =>
            new($"Value is incompatible with '{typeof(T)}' type.", paramName);
    }
}
