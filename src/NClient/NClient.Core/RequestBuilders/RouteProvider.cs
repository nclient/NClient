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
                throw OuterExceptionFactory.RouteParamWithoutTokenInRoute(clientName, methodName, routeParamNamesWithoutToken);

            var routeParts = new List<string>(routeTemplate.Segments.Count);
            foreach (var segment in routeTemplate.Segments)
            {
                var templatePart = segment.Parts.Single();
                var routePart = templatePart switch
                {
                    { } when templatePart.Name is not null => GetValueFromNamedSegment(templatePart, clientName, methodName, routeParams),
                    { } when templatePart.Text is not null => GetValueFromTextSegment(templatePart, clientName, methodName),
                    _ => throw OuterExceptionFactory.TemplatePartWithoutTokenOrText(clientName, methodName)
                };
                routeParts.Add(routePart);
            }

            return Path.Combine(routeParts.ToArray()).Replace('\\', '/');
        }

        private static string GetValueFromNamedSegment(TemplatePart templatePart, string clientName, string methodName, IEnumerable<Parameter> parameters)
        {
            var parameter = parameters.SingleOrDefault(x => x.Name == templatePart.Name);
            if (parameter is null)
                throw OuterExceptionFactory.TokenNotMatchAnyMethodParameter(clientName, methodName, templatePart.Name);
            if (!parameter.Type.IsSimple())
                throw OuterExceptionFactory.TemplatePartContainsComplexType(clientName, methodName, templatePart.Name);

            return parameter.Value?.ToString() ?? "";
        }

        private static string GetValueFromTextSegment(TemplatePart templatePart, string clientName, string methodName)
        {
            return templatePart.Text switch
            {
                "[controller]" => GetControllerName(clientName),
                "[action]" => methodName,
                { Length: > 2 } token when token.First() == '[' && token.Last() == ']' => 
                    throw OuterExceptionFactory.TokenFromTemplateNotExists(clientName, methodName, token),
                _ => templatePart.Text
            };
        }

        private static string GetControllerName(string clientName)
        {
            var name = clientName;

            //TODO: Check interface or not
            if (name.Length >= 3 && name[0] == 'I' && char.IsUpper(name[1]) && char.IsLower(name[2]))
                name = new string(name.Skip(1).ToArray());

            const string controllerSuffix = "Controller";
            if (name.Length > controllerSuffix.Length && name.EndsWith(controllerSuffix))
                name = name.Remove(name.Length - controllerSuffix.Length, controllerSuffix.Length);

            const string clientSuffix = "Client";
            if (name.Length > clientSuffix.Length && name.EndsWith(clientSuffix))
                name = name.Remove(name.Length - clientSuffix.Length, clientSuffix.Length);

            return name;
        }
    }
}
