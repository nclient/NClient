﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Annotations;
using NClient.Invocation;
using NClient.Standalone.ClientProxy.Generation.MethodBuilders.Providers;

namespace NClient.Standalone.ClientProxy.Generation.MethodBuilders
{
    internal interface IMethodParamBuilder
    {
        IMethodParam[] Build(MethodInfo method, IEnumerable<MethodInfo> overridingMethods);
    }

    internal class MethodParamBuilder : IMethodParamBuilder
    {
        private readonly IParamAttributeProvider _paramAttributeProvider;

        public MethodParamBuilder(IParamAttributeProvider paramAttributeProvider)
        {
            _paramAttributeProvider = paramAttributeProvider;
        }

        public IMethodParam[] Build(MethodInfo method, IEnumerable<MethodInfo> overridingMethods)
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
                .Cast<IMethodParam>()
                .ToArray();
        }

        private static string GetParamName(ParameterInfo paramInfo, IParamAttribute? paramAttribute)
        {
            return (paramAttribute as INameProviderAttribute)?.Name ?? paramInfo.Name;
        }
    }
}
