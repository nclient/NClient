using System;
using NClient.Common.Helpers;

namespace NClient.Annotations.Versioning
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ToVersionAttribute : Attribute
    {
        public string Version { get; }

        public ToVersionAttribute(string version)
        {
            Ensure.IsNotNullOrEmpty(version, nameof(version));

            Version = version;
        }
    }
}