using System;

namespace NClient.Annotations.Versioning
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class VersionAttribute : Attribute
    {
        public string Version { get; }
        
        public VersionAttribute(string version)
        {
            Version = version;
        }
    }
}