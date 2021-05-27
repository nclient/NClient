using System;

namespace NClient.Annotations.Versioning
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public class VersionAttribute : Attribute
    {
        public string Version { get; }
        public bool Deprecated { get; set; }
        
        public VersionAttribute(string version)
        {
            Version = version;
        }
    }
}