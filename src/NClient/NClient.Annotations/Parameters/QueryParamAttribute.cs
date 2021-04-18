using System;

namespace NClient.Annotations.Parameters
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class QueryParamAttribute : ParamAttribute
    {
        public string? Name { get; set; }
    }
}
