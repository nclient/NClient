using System;

namespace NClient.Standalone.ClientProxy.Interceptors.RequestBuilders.Models
{
    internal class MethodParameter
    {
        public string Name { get; }
        public Type Type { get; }
        public object? Value { get; }
        public Attribute Attribute { get; }

        public MethodParameter(string name, Type type, object? value, Attribute attribute)
        {
            Name = name;
            Type = type;
            Value = value;
            Attribute = attribute;
        }
    }
}
