// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Specifies that the class or method that this attribute is applied to requires the specified authorization.</summary>
    public interface IAuthorizedAttribute
    {
        /// <summary>Gets the policy name that determines access to the resource.</summary>
        string? Policy { get; }
        
        /// <summary>Gets or sets a comma delimited list of roles that are allowed to access the resource.</summary>
        string? Roles { get; set; }
        
        /// <summary>Gets or sets a comma delimited list of schemes from which user information is constructed.</summary>
        string? AuthenticationSchemes { get; set; }
    }
}
