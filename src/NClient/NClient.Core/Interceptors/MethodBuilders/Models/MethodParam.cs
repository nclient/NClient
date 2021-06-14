using System;
using NClient.Annotations.Parameters;

namespace NClient.Core.Interceptors.MethodBuilders.Models
{
    public class MethodParam
    {
        public string Name { get; }
        public Type Type { get; }
        public ParamAttribute Attribute { get; }

        public MethodParam(string name, Type type, ParamAttribute attribute)
        {
            Name = name;
            Type = type;
            Attribute = attribute;
        }
    }
}