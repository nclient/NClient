using System;

// ReSharper disable once CheckNamespace
namespace NClient.Annotations
{
    /// <summary>Specifies that a parameter should be pass an data in an transport message. Many parameters are allowed.</summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PropertyParamAttribute : ParamAttribute, IPropertyParamAttribute
    {
        /// <summary>Gets or sets parameter name.</summary>
        public string? Name { get; set; }
    }
}
