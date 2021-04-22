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
            var pathAttributes = (clientType.IsInterface
                ? clientType.GetInterfaceCustomAttributes(inherit: true)
                : clientType.GetCustomAttributes(inherit: true).Cast<Attribute>())
                .Select(x => _attributeMapper.TryMap(x))
                .Where(x => x is PathAttribute)
                .Cast<PathAttribute>()
                .ToArray();
            if (pathAttributes.Length > 1)
                throw OuterExceptionFactory.MultipleAttributeForClientNotSupported(clientType.Name, nameof(PathAttribute));
            var pathAttribute = pathAttributes.SingleOrDefault();

            //TODO: Duplication here and in HttpMethodProvider
            var methodAttributes = method
                .GetCustomAttributes()
                .Select(x => _attributeMapper.TryMap(x))
                .Where(x => x is MethodAttribute)
                .Cast<MethodAttribute>()
                .ToArray();
            if (methodAttributes.Length > 1)
                throw OuterExceptionFactory.MultipleMethodAttributeNotSupported(method);
            var methodAttribute = methodAttributes.SingleOrDefault()
                ?? throw OuterExceptionFactory.MethodAttributeNotFound(typeof(MethodAttribute), method);

            var baseTemplate = pathAttribute?.Template ?? "";
            var methodTemplate = methodAttribute.Template ?? "";
            var fullTemplate = Path.IsPathRooted(methodTemplate)
                ? methodTemplate
                : UriCombine(baseTemplate, methodTemplate);

            return Parse(fullTemplate);
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

        private static string UriCombine(string left, string right)
        {
            return $"{left.TrimEnd('/')}/{right.TrimStart('/')}";
        }
    }
}
