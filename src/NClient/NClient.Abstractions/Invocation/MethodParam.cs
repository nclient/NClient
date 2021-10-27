using System;
using NClient.Annotations;

namespace NClient.Invocation
{
    // TODO: doc
    public class MethodParam
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
