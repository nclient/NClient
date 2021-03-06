using System;
using System.IO;
using NClient.Core.AspNetRouting;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers;
using NClient.Core.Interceptors.MethodBuilders.Models;

namespace NClient.Core.Interceptors.RequestBuilders
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
                : UriHelper.Combine(baseTemplate, methodTemplate);

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
    }
}
