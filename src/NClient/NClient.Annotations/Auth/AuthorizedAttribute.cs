using System;
using NClient.Common.Helpers;

namespace NClient.Annotations.Auth
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizedAttribute : Attribute
    {
        public string? Policy { get; }
        public string? Roles { get; set; }
        public string? AuthenticationSchemes { get; set; }

        public AuthorizedAttribute()
        {
        }

        public AuthorizedAttribute(string policy)
        {
            Ensure.IsNotNullOrEmpty(policy, nameof(policy));
            
            Policy = policy;
        }
    }
}