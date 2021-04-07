using System.Collections.Generic;
using System.IO;
using System.Linq;
using NClient.Annotations.Parameters;
using NClient.Core.AspNetRouting;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers;
using NClient.Core.RequestBuilders.Models;

namespace NClient.Core.RequestBuilders
{
    internal interface IRouteProvider
    {
        string Build(RouteTemplate routeTemplate, string clientName, string methodName, Parameter[] parameters);
    }

    internal class RouteProvider : IRouteProvider
    {
        private static readonly string[] Suffixes = new[] { "Controller", "Facade", "Client" };

        public string Build(RouteTemplate routeTemplate, string clientName, string methodName, Parameter[] parameters)
        {
            var routeParams = parameters
                .Where(x => x.Attribute is RouteParamAttribute)
                .ToArray();
            var routeParamNamesWithoutToken = routeParams
                .Select(x => x.Name)
                .Except(routeTemplate.Parameters.Select(x => x.Name))
                .ToArray();
            if (routeParamNamesWithoutToken.Any())
                throw OuterExceptionFactory.RouteParamWithoutTokenInRoute(clientName, methodName, routeParamNamesWithoutToken!);

            var routeParts = new List<string>(routeTemplate.Segments.Count);
            foreach (var segment in routeTemplate.Segments)
            {
                var templatePart = segment.Parts.Single();
                var routePart = templatePart switch
                {
                    { } when templatePart.Name is not null => GetValueFromNamedSegment(templatePart, clientName, methodName, routeParams, parameters),
                    { } when templatePart.Text is not null => GetValueFromTextSegment(templatePart, clientName, methodName),
                    _ => throw OuterExceptionFactory.TemplatePartWithoutTokenOrText(clientName, methodName)
                };
                routeParts.Add(routePart);
            }

            return Path.Combine(routeParts.ToArray()).Replace('\\', '/');
        }

        private static string GetValueFromNamedSegment(
            TemplatePart templatePart, string clientName, string methodName, IEnumerable<Parameter> routeParameters, IEnumerable<Parameter> allParameter)
        {
            var (objectName, memberPath) = ObjectMemberManager.ParseNextPath(templatePart.Name!);

            if (memberPath is null)
            {
                var parameter = routeParameters.SingleOrDefault(x => x.Name == objectName);
                if (parameter is null)
                    throw OuterExceptionFactory.TokenNotMatchAnyMethodParameter(clientName, methodName, templatePart.Name!);
                if (!parameter.Type.IsSimple())
                    throw OuterExceptionFactory.TemplatePartContainsComplexType(clientName, methodName, templatePart.Name!);

                return parameter.Value?.ToString() ?? "";
            }
            else
            {
                var parameter = allParameter.SingleOrDefault(x => x.Name == objectName);
                if (parameter is null)
                    throw OuterExceptionFactory.TokenNotMatchAnyMethodParameter(clientName, methodName, templatePart.Name!);
                if (parameter.Value is null)
                    throw OuterExceptionFactory.ParameterInRouteTemplateIsNull(parameter.Name);

                return ObjectMemberManager.GetMemberValue(parameter.Value, memberPath)?.ToString() ?? "";
            }
        }

        private static string GetValueFromTextSegment(TemplatePart templatePart, string clientName, string methodName)
        {
            return templatePart.Text switch
            {
                "[controller]" => GetControllerName(clientName),
                "[action]" => methodName,
                { Length: > 2 } token when token.First() == '[' && token.Last() == ']' =>
                    throw OuterExceptionFactory.TokenFromTemplateNotExists(clientName, methodName, token),
                _ => templatePart.Text ?? throw InnerExceptionFactory.ArgumentException($"{nameof(templatePart.Text)} from {templatePart} is null.", nameof(templatePart))
            };
        }

        private static string GetControllerName(string clientName)
        {
            clientName = GetNameWithoutPrefix(clientName);
            clientName = GetNameWithoutSuffix(clientName);
            return clientName;
        }

        private static string GetNameWithoutPrefix(string name)
        {
            //TODO: Check interface or not
            if (name.Length >= 3 && name[0] == 'I' && char.IsUpper(name[1]) && char.IsLower(name[2]))
                return new string(name.Skip(1).ToArray());

            return name;
        }

        private static string GetNameWithoutSuffix(string name)
        {
            foreach (var suffix in Suffixes)
            {
                if (name.Length > suffix.Length && name.EndsWith(suffix))
                    return name.Remove(name.Length - suffix.Length, suffix.Length);
            }

            return name;
        }
    }
}
