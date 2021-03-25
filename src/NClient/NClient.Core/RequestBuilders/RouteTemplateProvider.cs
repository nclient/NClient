using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Core.AspNetRouting;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers;
using NClient.Core.Mappers;

namespace NClient.Core.RequestBuilders
{
    internal interface IRouteTemplateProvider
    {
        RouteTemplate Get(Type clientType, MethodInfo method);
    }

    internal class RouteTemplateProvider : IRouteTemplateProvider
    {
        private readonly IAttributeMapper _attributeMapper;

        public RouteTemplateProvider(IAttributeMapper attributeMapper)
        {
            _attributeMapper = attributeMapper;
        }

        public RouteTemplate Get(Type clientType, MethodInfo method)
        {
            var apiAttributes = (clientType.IsInterface 
                ? clientType.GetInterfaceCustomAttributes(inherit: true) 
                : clientType.GetCustomAttributes(inherit: true).Cast<Attribute>())
                .Select(x => _attributeMapper.TryMap(x))
                .Where(x => x is PathAttribute)
                .ToArray();
            if (apiAttributes.Length > 1)
                throw OuterExceptionFactory.MultipleAttributeForClientNotSupported(clientType.Name, nameof(PathAttribute));
            var apiAttribute = apiAttributes.SingleOrDefault();

            //TODO: Duplication here and in HttpMethodProvider
            var methodAttributes = method
                .GetCustomAttributes(inherit: true)
                .Cast<Attribute>()
                .Select(x => _attributeMapper.TryMap(x))
                .Where(x => x is MethodAttribute)
                .ToArray();
            if (methodAttributes.Length > 1)
                throw OuterExceptionFactory.MultipleMethodAttributeNotSupported(method);
            var methodAttribute = methodAttributes.SingleOrDefault()
                ?? throw OuterExceptionFactory.MethodAttributeNotFound(typeof(MethodAttribute), method);

            var apiTemplate = apiAttribute?.GetType().GetProperty("Template")!.GetValue(apiAttribute, null) as string ?? "";
            var methodTemplate = methodAttribute.GetType().GetProperty("Template")!.GetValue(methodAttribute, null) as string ?? "";
            var routeTemplateStr = Path.IsPathRooted(methodTemplate) 
                ? methodTemplate
                : UriCombine(apiTemplate, methodTemplate);

            return Parse(routeTemplateStr);
        }

        private static RouteTemplate Parse(string routeTemplateStr)
        {
            try
            {
                return TemplateParser.Parse(routeTemplateStr);
            }
            catch (ArgumentException e)
            {
                throw OuterExceptionFactory.TemplateParsingError(e);
            }
        }

        public static string UriCombine(string left, string right)
        {
            left = left.TrimEnd('/');
            right = right.TrimStart('/');
            return $"{left}/{right}";
        }
    }
}
