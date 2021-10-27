using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Annotations;
using NClient.Invocation;
using NClient.Standalone.ClientProxy.Interceptors.MethodBuilders.Providers;

namespace NClient.Standalone.ClientProxy.Interceptors.MethodBuilders
{
    internal interface IMethodParamBuilder
    {
        MethodParam[] Build(MethodInfo method, IEnumerable<MethodInfo> overridingMethods);
    }

    internal class MethodParamBuilder : IMethodParamBuilder
    {
        private readonly IParamAttributeProvider _paramAttributeProvider;

        public MethodParamBuilder(IParamAttributeProvider paramAttributeProvider)
        {
            _paramAttributeProvider = paramAttributeProvider;
        }

        public MethodParam[] Build(MethodInfo method, IEnumerable<MethodInfo> overridingMethods)
        {
            return method
                .GetParameters()
                .Select(x =>
                {
                    var overridingParams = overridingMethods
                        .Select(overridingMethod => overridingMethod
                            .GetParameters().Single(overridingParam => overridingParam.Name == x.Name))
                        .ToArray();
                    var paramAttribute = _paramAttributeProvider.Get(x, overridingParams);
                    var paramName = GetParamName(x, paramAttribute);
                    var paramType = x.ParameterType;
                    return new MethodParam(paramName, paramType, paramAttribute);
                })
                .ToArray();
        }

        private static string GetParamName(ParameterInfo paramInfo, IParamAttribute? paramAttribute)
        {
            return (paramAttribute as INameProviderAttribute)?.Name ?? paramInfo.Name;
        }
    }
}
