using System;
using System.Linq;
using System.Reflection;
using NClient.Annotations.Parameters;
using NClient.Core.AspNetRouting;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers;
using NClient.Core.Mappers;
using NClient.Core.RequestBuilders.Models;

namespace NClient.Core.RequestBuilders
{
    internal interface IParameterProvider
    {
        Parameter[] Get(RouteTemplate routeTemplate, MethodInfo method, object[] arguments);
    }

    internal class ParameterProvider : IParameterProvider
    {
        private readonly IAttributeMapper _attributeMapper;

        public ParameterProvider(IAttributeMapper attributeMapper)
        {
            _attributeMapper = attributeMapper;
        }

        public Parameter[] Get(RouteTemplate routeTemplate, MethodInfo method, object[] arguments)
        {
            return method
                .GetParameters()
                .Select((paramInfo, index) =>
                {
                    var paramAttributes = paramInfo.GetCustomAttributes()
                        .Select(attribute => _attributeMapper.TryMap(attribute))
                        .Where(x => x is ParamAttribute)
                        .Cast<ParamAttribute>()
                        .ToArray();
                    if (paramAttributes.Length > 1)
                        throw OuterExceptionFactory.MultipleParameterAttributeNotSupported(method, paramInfo.Name);
                    var paramAttribute = paramAttributes.SingleOrDefault();

                    return new Parameter(
                        GetParamName(paramInfo, paramAttribute),
                        paramInfo.ParameterType,
                        arguments.ElementAtOrDefault(index),
                        paramAttribute ?? GetAttributeForImplicitParameter(paramInfo, routeTemplate));
                })
                .ToArray();
        }

        private static string GetParamName(ParameterInfo paramInfo, ParamAttribute? paramAttribute)
        {
            var nameFromAttribute = paramAttribute?
                .GetType()
                .GetProperty("Name", typeof(string))?
                .GetValue(paramAttribute) as string;

            return nameFromAttribute ?? paramInfo.Name;
        }

        private static Attribute GetAttributeForImplicitParameter(ParameterInfo paramInfo, RouteTemplate routeTemplate)
        {
            if (routeTemplate.Parameters.Any(x => x.Name == paramInfo.Name))
                return new RouteParamAttribute();

            return paramInfo.ParameterType.IsPrimitive()
                ? new QueryParamAttribute()
                : new BodyParamAttribute();
        }
    }
}
