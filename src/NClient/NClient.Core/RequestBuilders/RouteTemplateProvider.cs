using System;
using System.IO;
using NClient.Core.AspNetRouting;
using NClient.Core.Exceptions.Factories;
using NClient.Core.MethodBuilders.Models;

namespace NClient.Core.RequestBuilders
{
    internal interface IRouteTemplateProvider
    {
        RouteTemplate Get(Method method);
    }

    internal class RouteTemplateProvider : IRouteTemplateProvider
    {
        public RouteTemplate Get(Method method)
        {
            var baseTemplate = method.PathAttribute?.Template ?? "";
            var methodTemplate = method.Attribute.Template ?? "";
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
