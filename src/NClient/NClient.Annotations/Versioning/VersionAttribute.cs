using System;
using NClient.Common.Helpers;

namespace NClient.Annotations.Versioning
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public class VersionAttribute : Attribute
    {
        public string Version { get; }
        public bool Deprecated { get; set; }

        public VersionAttribute(string version)
        {
            Ensure.IsNotNullOrEmpty(version, nameof(version));

            Version = version;
        }
    }
}