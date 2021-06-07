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
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;

        public RouteTemplateProvider(IClientValidationExceptionFactory clientValidationExceptionFactory)
        {
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
        }

        public RouteTemplate Get(Method method)
        {
            var baseTemplate = method.PathAttribute?.Template ?? "";
            var methodTemplate = method.Attribute.Template ?? "";
            var fullTemplate = Path.IsPathRooted(methodTemplate)
                ? methodTemplate
                : UriCombine(baseTemplate, methodTemplate);

            return Parse(fullTemplate);
        }

        private RouteTemplate Parse(string routeTemplateStr)
        {
            try
            {
                return TemplateParser.Parse(routeTemplateStr);
            }
            catch (ArgumentException e)
            {
                throw _clientValidationExceptionFactory.TemplateParsingError(e);
            }
        }

        private static string UriCombine(string left, string right)
        {
            return $"{left.TrimEnd('/')}/{right.TrimStart('/')}";
        }
    }
}
