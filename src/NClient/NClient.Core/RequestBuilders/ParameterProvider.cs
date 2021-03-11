using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Routing.Template;
using NClient.Core.Attributes;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers;
using NClient.Core.RequestBuilders.Models;

namespace NClient.Core.RequestBuilders
{
    internal interface IParameterProvider
    {
        Parameter[] Get(RouteTemplate routeTemplate, MethodInfo method, object[] arguments);
    }

    internal class ParameterProvider : IParameterProvider
    {
        private readonly IAttributeHelper _attributeHelper;

        public ParameterProvider(IAttributeHelper attributeHelper)
        {
            _attributeHelper = attributeHelper;
        }

        public Parameter[] Get(RouteTemplate routeTemplate, MethodInfo method, object[] arguments)
        {
            return method
                .GetParameters()
                .Select((paramInfo, index) =>
                {
                    var paramAttributes = paramInfo.GetCustomAttributes()
                        .Select(attribute => _attributeHelper.IsNotSupportedMethodAttribute(attribute)
                            ? throw OuterExceptionFactory.UsedNotSupportedAttributeForParameter(method, paramInfo.Name, attribute.GetType().Name) : attribute)
                        .Where(_attributeHelper.IsParameterAttribute)
                        .ToArray();
                    if (paramAttributes.Length > 1)
                        throw OuterExceptionFactory.MultipleParameterAttributeNotSupported(method, paramInfo.Name);
                    var paramAttribute = paramAttributes.SingleOrDefault();

                    return new Parameter(
                        paramInfo.Name,
                        arguments.ElementAtOrDefault(index),
                        paramAttribute ?? GetAttributeForImplicitParameter(paramInfo, routeTemplate));
                })
                .ToArray();
        }

        private Attribute GetAttributeForImplicitParameter(ParameterInfo paramInfo, RouteTemplate routeTemplate)
        {
            if (routeTemplate.Parameters.Any(x => x.Name == paramInfo.Name))
                return _attributeHelper.CreateAttributeInstance(_attributeHelper.RouteParamAttributeType);

            return paramInfo.ParameterType.IsSimple()
                ? _attributeHelper.CreateAttributeInstance(_attributeHelper.UriParamAttributeType)
                : _attributeHelper.CreateAttributeInstance(_attributeHelper.BodyParamAttributeType);
        }
    }
}
