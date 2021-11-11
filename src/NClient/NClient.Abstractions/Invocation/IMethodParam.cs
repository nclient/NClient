using System;
using NClient.Annotations;

namespace NClient.Invocation
{
    // TODO: doc
    public interface IMethodParam
    {
        string Name { get; }
        Type Type { get; }
        IParamAttribute Attribute { get; }
    }
}
