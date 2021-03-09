using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Routing.Template;
using MoreLinq.Extensions;
using NClient.Core.Attributes;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers;

namespace NClient.Core.RequestBuilders
{
    internal interface IRouteBuilder
    {
        string Build(Type clientType, MethodInfo method, object[] arguments);
    }

    internal class RouteBuilder : IRouteBuilder
    {
        private readonly IAttributeHelper _attributeHelper;

        public RouteBuilder(IAttributeHelper attributeHelper)
        {
            _attributeHelper = attributeHelper;
        }

        public string Build(Type clientType, MethodInfo method, object[] arguments)
        {
            var routeTemplate = GetRouteTemplate(clientType, method);
            var routeTemplateParams = GetRouteTemplateParams(method, arguments);
            return BuildRoute(clientType, method, routeTemplate, routeTemplateParams);
        }

        private string GetRouteTemplate(Type clientType, MethodInfo method)
        {
            var apiAttributes = clientType
                .GetCustomAttributes(_attributeHelper.ApiAttributeType)
                .ToArray();
            if (apiAttributes.Length > 1)
                throw OuterExceptionFactory.MultipleAttributeForClientNotSupported(clientType.Name, _attributeHelper.ApiAttributeType.Name);
            var apiAttribute = apiAttributes .SingleOrDefault()
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

            return string.Join('/',
                apiAttribute.GetType().GetProperty("Template")!.GetValue(apiAttribute, null) as string ?? "",
                methodAttribute.GetType().GetProperty("Template")!.GetValue(methodAttribute, null) as string ?? "");
        }

        private static IReadOnlyDictionary<string, object> GetRouteTemplateParams(MethodInfo method, IEnumerable<object> arguments)
        {
            return method.GetParameters()
                .ZipLongest(arguments, (x, y) => (Name: x.Name!, Value: y))
                .ToDictionary(x => x.Name, x => x.Value); ;
        }

        private static string BuildRoute(Type clientType, MethodInfo method, string routeTemplate, IReadOnlyDictionary<string, object> routeTemplateParams)
        {
            var template = ParseRouteTemplate(routeTemplate);
            var routeParts = new List<string>(template.Segments.Count);
            foreach (var segment in template.Segments)
            {
                var templatePart = segment.Parts.Single();
                var routePart = templatePart switch
                {
                    { } when templatePart.Name is not null => GetValueFromNamedSegment(clientType, method, templatePart, routeTemplateParams),
                    { } when templatePart.Text is not null => GetValueFromTextSegment(templatePart, clientType, method),
                    _ => throw OuterExceptionFactory.TemplatePartWithoutTokenOrText(method)
                };
                routeParts.Add(routePart);
            }

            return Path.Combine(routeParts.ToArray()).Replace('\\', '/');
        }

        private static string GetValueFromNamedSegment(Type clientType, MethodInfo method, TemplatePart templatePart, IReadOnlyDictionary<string, object> routeTemplateParams)
        {
            if (!routeTemplateParams.TryGetValue(templatePart.Name, out var value))
                throw OuterExceptionFactory.TokenNotMatchAnyMethodParameter(method, templatePart.Name);
            if (!value.GetType().IsSimple())
                throw OuterExceptionFactory.TemplatePartContainsComplexType(method, templatePart.Name);

            return value.ToString()!;
        }

        private static string GetValueFromTextSegment(TemplatePart templatePart, Type clientType, MethodInfo method)
        {
            return templatePart.Text switch
            {
                "[controller]" => GetControllerName(clientType),
                "[action]" => method.Name,
                { Length: > 2 } token when token.First() == '[' && token.Last() == ']' => 
                    throw OuterExceptionFactory.TokenFromTemplateNotExists(method, token),
                _ => templatePart.Text
            };
        }

        private static string GetControllerName(Type clientType)
        {
            var name = clientType.Name;

            if (clientType.IsInterface && name.Length >= 2 && name[0] == 'I' && char.IsUpper(name[1]))
                name = new string(name.Skip(1).ToArray());

            if (new string(Enumerable.TakeLast(name, 10).ToArray()).Equals("Controller", StringComparison.Ordinal))
                name = new string(Enumerable.SkipLast(name, 10).ToArray());

            return name;
        }

        private static RouteTemplate ParseRouteTemplate(string routeTemplate)
        {
            try
            {
                return TemplateParser.Parse(routeTemplate);
            }
            catch (ArgumentException e)
            {
                throw OuterExceptionFactory.TemplateParsingError(e);
            }
        }
    }
}
