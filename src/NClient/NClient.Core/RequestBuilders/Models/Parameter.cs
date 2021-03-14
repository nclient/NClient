using System;

namespace NClient.Core.RequestBuilders.Models
{
    internal class Parameter
    {
        public string Name { get; }
        public Type Type { get; set; }
        public object? Value { get; }
        public Attribute Attribute { get; }

        public Parameter(string name, Type type, object? value, Attribute attribute)
        {
            Name = name;
            Type = type;
            Value = value;
            Attribute = attribute;
        }
    }
}
