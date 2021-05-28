using System;
using System.Diagnostics;

namespace NClient.Common.Helpers
{
    [DebuggerStepThrough]
    internal static class Ensure
    {
        public static T IsNotNull<T>(T value, string paramName) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(paramName, "Value cannot be null.");
            return value;
        }

        public static string IsNotNullOrEmpty(string value, string paramName)
        {
            if (value == null)
                throw new ArgumentNullException(paramName);

            if (value.Length == 0)
                throw new ArgumentException("Value cannot be empty.", paramName);

            return value;
        }

        public static T IsCompatibleWith<T>(object value, string paramName) where T : class
        {
            if (value is not T)
                throw new ArgumentException($"Value is compatible with '{typeof(T)}' type.", paramName);
            return (T)value;
        }
    }
}