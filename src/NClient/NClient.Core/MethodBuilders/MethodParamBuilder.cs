using System.Linq;
using System.Reflection;
using NClient.Annotations.Abstractions;
using NClient.Annotations.Parameters;
using NClient.Core.MethodBuilders.Models;
using NClient.Core.MethodBuilders.Providers;

namespace NClient.Core.MethodBuilders
{
    internal interface IMethodParamBuilder
    {
        MethodParam[] Build(MethodInfo method);
    }

    internal class MethodParamBuilder : IMethodParamBuilder
    {
        private readonly IParamAttributeProvider _paramAttributeProvider;

        public MethodParamBuilder(IParamAttributeProvider paramAttributeProvider)
        {
            _paramAttributeProvider = paramAttributeProvider;
        }

        public MethodParam[] Build(MethodInfo method)
        {
            return method
                .GetParameters()
                .Select(x =>
                {
                    var paramAttribute = _paramAttributeProvider.Get(x);
                    var paramName = GetParamName(x, paramAttribute);
                    var paramType = x.ParameterType;
                    return new MethodParam(paramName, paramType, paramAttribute);
                })
                .ToArray();
        }

        private static string GetParamName(ParameterInfo paramInfo, ParamAttribute? paramAttribute)
        {
            return (paramAttribute as INameProviderAttribute)?.Name ?? paramInfo.Name;
        }
    }
}
