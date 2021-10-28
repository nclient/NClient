using System;
using NClient.Common.Helpers;

namespace NClient.Annotations.Auth
{
    /// <summary>
    /// Specifies that the class or method that this attribute is applied to requires the specified authorization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizedAttribute : Attribute, IAuthorizedAttribute
    {
        /// <summary>
        /// Gets the policy name that determines access to the resource.
        /// </summary>
        public string? Policy { get; }

        /// <summary>
        /// Gets or sets a comma delimited list of roles that are allowed to access the resource.
        /// </summary>
        public string? Roles { get; set; }

        /// <summary>
        /// Gets or sets a comma delimited list of schemes from which user information is constructed.
        /// </summary>
        public string? AuthenticationSchemes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizedAttribute" /> class.
        /// </summary>
        public AuthorizedAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizedAttribute" /> class with the specified policy.
        /// </summary>
        /// <param name="policy">The name of the policy to require for authorization.</param>
        public AuthorizedAttribute(string policy)
        {
            Ensure.IsNotNullOrEmpty(policy, nameof(policy));

            Policy = policy;
        }
    }
}
