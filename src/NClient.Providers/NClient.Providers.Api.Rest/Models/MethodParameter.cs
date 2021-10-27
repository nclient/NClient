using System;
using NClient.Annotations;

namespace NClient.Providers.Api.Rest.Models
{
    internal class MethodParameter
    {
        public string Name { get; }
        public Type Type { get; }
        public object? Value { get; }
        public IParamAttribute Attribute { get; }

        public MethodParameter(string name, Type type, object? value, IParamAttribute attribute)
        {
            Name = name;
            Type = type;
            Value = value;
            Attribute = attribute;
        }
    }
}
