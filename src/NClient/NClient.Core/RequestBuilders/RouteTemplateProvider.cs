using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Routing.Template;
using NClient.Core.Attributes;
using NClient.Core.Exceptions.Factories;

namespace NClient.Core.RequestBuilders
{
    public interface IRouteTemplateProvider
    {
        RouteTemplate Get(Type clientType, MethodInfo method);
    }

    public class RouteTemplateProvider : IRouteTemplateProvider
    {
        private readonly IAttributeHelper _attributeHelper;

        public RouteTemplateProvider(IAttributeHelper attributeHelper)
        {
            _attributeHelper = attributeHelper;
        }

        public RouteTemplate Get(Type clientType, MethodInfo method)
        {
            var apiAttributes = clientType
                .GetCustomAttributes(_attributeHelper.ApiAttributeType)
                .ToArray();
            if (apiAttributes.Length > 1)
                throw OuterExceptionFactory.MultipleAttributeForClientNotSupported(clientType.Name, _attributeHelper.ApiAttributeType.Name);
            var apiAttribute = apiAttributes.SingleOrDefault()
                ?? throw OuterExceptionFactory.ClientAttributeNotFound(_attributeHelper.ApiAttributeType, clientType);

            //TODO: Duplication here and in HttpMethodProvider
            var methodAttributes = method
                .GetCustomAttributes()
                .Select(x => _attributeHelper.IsNotSupportedMethodAttribute(x)
                    ? throw OuterExceptionFactory.MethodAttributeNotSupported(method, x.GetType().Name) : x)
                .Where(x => x.GetType().IsAssignableTo(_attributeHelper.MethodAttributeType))
                .ToArray();
            if (methodAttributes.Length > 1)
                throw OuterExceptionFactory.MultipleMethodAttributeNotSupported(method);
            var methodAttribute = methodAttributes.SingleOrDefault()
                ?? throw OuterExceptionFactory.MethodAttributeNotFound(_attributeHelper.MethodAttributeType, method);

            var routeTemplateStr = string.Join('/',
                apiAttribute.GetType().GetProperty("Template")!.GetValue(apiAttribute, null) as string ?? "",
                methodAttribute.GetType().GetProperty("Template")!.GetValue(methodAttribute, null) as string ?? "");

            return Parse(routeTemplateStr);
        }

        private RouteTemplate Parse(string routeTemplateStr)
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
    }
}
