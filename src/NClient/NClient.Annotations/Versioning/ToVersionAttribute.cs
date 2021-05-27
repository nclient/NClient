using System;

namespace NClient.Annotations.Versioning
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ToVersionAttribute : Attribute
    {
        public string Version { get; }

        public ToVersionAttribute(string version)
        {
            Version = version;
        }
    }
}