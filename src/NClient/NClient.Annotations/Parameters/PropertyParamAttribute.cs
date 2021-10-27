using System;

// ReSharper disable once CheckNamespace
namespace NClient.Annotations
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PropertyParamAttribute : ParamAttribute, IPropertyParamAttribute
    {
        /// <summary>
        /// Property name.
        /// </summary>
        public string? Name { get; set; }
    }
}
