using System;
using NClient.Common.Helpers;

namespace NClient.Annotations.Versioning
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class UseVersionAttribute : Attribute
    {
        public string Version { get; }

        public UseVersionAttribute(string version)
        {
            Ensure.IsNotNullOrEmpty(version, nameof(version));
            
            Version = version;
        }
    }
}