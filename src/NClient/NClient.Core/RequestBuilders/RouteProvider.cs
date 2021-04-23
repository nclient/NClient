using System.Collections.Generic;
using System.IO;
using System.Linq;
using NClient.Annotations.Parameters;
using NClient.Core.AspNetRouting;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers;
using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Helpers.ObjectMemberManagers.MemberNameSelectors;
using NClient.Core.RequestBuilders.Models;

namespace NClient.Core.RequestBuilders
{
    internal interface IRouteProvider
    {
        string Build(
            RouteTemplate routeTemplate, 
            string clientName, 
            string methodName, 
            Parameter[] parameters);
    }

    internal class RouteProvider : IRouteProvider
    {
        private static readonly string[] Suffixes = new[] { "Controller", "Facade", "Client" };

        public string Build(
            RouteTemplate routeTemplate, 
            string clientName, 
            string methodName, 
            Parameter[] parameters)
        {
            var unusedRouteParamNames = parameters
                .Where(x => x.Attribute is RouteParamAttribute)
                .Select(x => x.Name)
                .Except(routeTemplate.Parameters.Select(x => x.Name))
                .ToArray();
            if (unusedRouteParamNames.Any())
                throw OuterExceptionFactory.RouteParamWithoutTokenInRoute(clientName, methodName, unusedRouteParamNames!);

            var routeParts = routeTemplate.Segments
                .Select(x => x.Parts.Single() switch
                {
                    { Name: { } } templatePart => GetValueFromPartName(templatePart, parameters),
                    { Text: { } } templatePart => GetValueFromPartText(templatePart, clientName, methodName),
                    _ => throw OuterExceptionFactory.TemplatePartWithoutTokenOrText(clientName, methodName)
                });

            return Path.Combine(routeParts.ToArray()).Replace('\\', '/');
        }
        
        private static string GetValueFromPartName(TemplatePart templatePart, Parameter[] parameters)
        {
            var (objectName, memberPath) = ObjectMemberManager.ParseNextPath(templatePart.Name!);
            return memberPath is null 
                ? GetParameterValue(objectName, parameters) 
                : GetCustomParameterValue(objectName, memberPath, parameters);
        }
        
        private static string GetParameterValue(string name, Parameter[] parameters)
        {
            var parameter = GetRouteParameter(name, parameters);
            if (!parameter.Value!.GetType().IsPrimitive())
                throw OuterExceptionFactory.TemplatePartContainsComplexType(name);
            
            return parameter.Value.ToString() ?? "";
        }
        
        private static string GetCustomParameterValue(string objectName, string memberPath, Parameter[] parameters)
        {
            var parameter = GetRouteParameter(objectName, parameters);

            return (parameter.Attribute switch
            {
                BodyParamAttribute => ObjectMemberManager.GetMemberValue(parameter.Value!, memberPath, new BodyMemberNameSelector()),
                QueryParamAttribute => ObjectMemberManager.GetMemberValue(parameter.Value!, memberPath, new QueryMemberNameSelector()),
                { } => ObjectMemberManager.GetMemberValue(parameter.Value!, memberPath, new DefaultMemberNameSelector()),
                _ => throw InnerExceptionFactory.NullReference($"Parameter '{parameter.Name}' has no attribute.")
            })?.ToString() ?? "";
        }
        
        private static Parameter GetRouteParameter(string name, IEnumerable<Parameter> parameters)
        {
            var parameterValue = parameters.SingleOrDefault(x => x.Name == name);
            if (parameterValue is null)
                throw OuterExceptionFactory.TokenNotMatchAnyMethodParameter(name);
            if (parameterValue.Value is null)
                throw OuterExceptionFactory.ParameterInRouteTemplateIsNull(name);
            return parameterValue!;
        }

        private static string GetValueFromPartText(TemplatePart templatePart, string clientName, string methodName)
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

        private static string GetControllerName(string name)
        {
            var controllerName = GetNameWithoutSuffix(GetNameWithoutPrefix(name));
            if (string.IsNullOrEmpty(controllerName))
                throw OuterExceptionFactory.ClientNameConsistsOnlyOfSuffixesAndPrefixes(name);
            return controllerName;
        }

        //TODO: Check interface or not
        private static string GetNameWithoutPrefix(string name)
        {
            const string prefix = "I";
            if (name.StartsWith(prefix) && name.Length >= 3 && char.IsUpper(name[1]) && char.IsLower(name[2]))
                return name.Substring(prefix.Length, name.Length - prefix.Length);
            return name;
        }

        private static string GetNameWithoutSuffix(string name)
        {
            var suffix = Suffixes.FirstOrDefault(name.EndsWith);
            return suffix is null ? name : name.Remove(name.Length - suffix.Length, suffix.Length);
        }
    }
}
