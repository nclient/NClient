using System;

namespace NClient.Core.RequestBuilders.Models
{
    internal class Parameter
    {
        public string Name { get; }
        public object? Value { get; }
        public Attribute Attribute { get; }

        public Parameter(string name, object? value, Attribute attribute)
        {
            Name = name;
            Value = value;
            Attribute = attribute;
        }
    }
}
