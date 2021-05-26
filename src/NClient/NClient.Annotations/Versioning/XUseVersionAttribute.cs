using System;

namespace NClient.Annotations.Versioning
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class XUseVersionAttribute : Attribute
    {
        public string Version { get; }
        
        public XUseVersionAttribute(string version)
        {
            Version = version;
        }
    }
}