using System;

namespace NClient.Core.Attributes.Clients
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ApiVersionAttribute : Attribute
    {
        public string Version { get; }

        public ApiVersionAttribute(string version)
        {
            Version = version;
        }
    }
}
