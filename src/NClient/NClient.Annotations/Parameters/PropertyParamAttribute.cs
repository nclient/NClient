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

        /// <summary>Initializes a new <see cref="PropertyParamAttribute"/>.</summary>
        public PropertyParamAttribute()
        {
        }

        /// <summary>Initializes a new <see cref="PropertyParamAttribute"/> with the given property parameter.</summary>
        /// <param name="name">The property parameter name.</param>
        public PropertyParamAttribute(string name)
        {
            Name = name;
        }
    }
}
