using System;

namespace NClient.Annotations.Versioning
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public class XVersionAttribute : Attribute
    {
        public string Version { get; }
        public bool Deprecated { get; set; }
        
        public XVersionAttribute(string version)
        {
            Version = version;
        }
    }
}