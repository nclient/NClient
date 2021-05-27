using System;

namespace NClient.Annotations.Versioning
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class UseVersionAttribute : Attribute
    {
        public string Version { get; }

        public UseVersionAttribute(string version)
        {
            Version = version;
        }
    }
}