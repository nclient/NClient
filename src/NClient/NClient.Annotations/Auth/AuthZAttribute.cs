using System;

namespace NClient.Annotations.Auth
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthZAttribute : Attribute
    {
        public string? Policy { get; set; }
        public string? Roles { get; set; }
        public string? AuthenticationSchemes { get; set; }

        public AuthZAttribute()
        {
        }

        public AuthZAttribute(string policy)
        {
            Policy = policy;
        }
    }
}