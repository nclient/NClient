using System;
using NClient.Annotations;

namespace NClient.Invocation
{
    public class MethodParam : IMethodParam
    {
        public string Name { get; }
        public Type Type { get; }
        public IParamAttribute Attribute { get; }

        public MethodParam(string name, Type type, IParamAttribute attribute)
        {
            Name = name;
            Type = type;
            Attribute = attribute;
        }
    }
}
