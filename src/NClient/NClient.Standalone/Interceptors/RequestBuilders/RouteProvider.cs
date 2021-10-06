using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NClient.Annotations.Parameters;
using NClient.Annotations.Versioning;
using NClient.Core.Helpers;
using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Helpers.ObjectMemberManagers.MemberNameSelectors;
using NClient.Standalone.AspNetRouting;
using NClient.Standalone.Exceptions.Factories;
using NClient.Standalone.Interceptors.RequestBuilders.Models;

namespace NClient.Standalone.Interceptors.RequestBuilders
{
    internal interface IRouteProvider
    {
        string Build(
            RouteTemplate routeTemplate,
            string clientName,
            string methodName,
            Parameter[] parameters,
            UseVersionAttribute? useVersionAttribute);
    }

    internal class RouteProvider : IRouteProvider
    {
        private static readonly string[] Suffixes = new[] { "Controller", "Facade", "Client" };
        private readonly IObjectMemberManager _objectMemberManager;
        private readonly IClientArgumentExceptionFactory _clientArgumentExceptionFactory;
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;

        public RouteProvider(
            IObjectMemberManager objectMemberManager,
            IClientArgumentExceptionFactory clientArgumentExceptionFactory,
            IClientValidationExceptionFactory clientValidationExceptionFactory)
        {
            _objectMemberManager = objectMemberManager;
            _clientArgumentExceptionFactory = clientArgumentExceptionFactory;
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
        }

        public string Build(
            RouteTemplate routeTemplate,
            string clientName,
            string methodName,
            Parameter[] parameters,
            UseVersionAttribute? useVersionAttribute)
        {
            var unusedRouteParamNames = parameters
                .Where(x => x.Attribute is RouteParamAttribute)
                .Select(x => x.Name)
                .Except(routeTemplate.Parameters.Select(x => x.Name))
                .ToArray();
            if (unusedRouteParamNames.Any())
                throw _clientValidationExceptionFactory.RouteParamWithoutTokenInRoute(unusedRouteParamNames!);

            var routeParts = routeTemplate.Segments
                .Select(x =>
                {
                    var partValues = new List<string>();
                    foreach (var part in x.Parts)
                    {
                        var partValue = part switch
                        {
                            { Name: "version" } templatePart when templatePart.InlineConstraints.Any(constraint => constraint.Constraint == "apiVersion")
                                => GetValueForVersionToken(useVersionAttribute),
                            { Name: { } } templatePart
                                => GetValueForToken(templatePart, parameters),
                            { Text: { } } templatePart
                                => GetValueForText(templatePart, clientName, methodName),
                            _ => throw _clientValidationExceptionFactory.TemplatePartWithoutTokenOrText()
                        };
                        partValues.Add(partValue);
                    }
                    return string.Join("", partValues);
                });

            return Path.Combine(routeParts.ToArray()).Replace('\\', '/');
        }

        private string GetValueForVersionToken(UseVersionAttribute? versionAttribute)
        {
            if (versionAttribute is null)
                throw _clientValidationExceptionFactory.UsedVersionTokenButVersionAttributeNotFound();
            return versionAttribute.Version;
        }

        private string GetValueForToken(TemplatePart templatePart, Parameter[] parameters)
        {
            var (objectName, memberPath) = _objectMemberManager.ParseNextPath(templatePart.Name!);
            return memberPath is null
                ? GetParameterValue(objectName, parameters)
                : GetCustomParameterValue(objectName, memberPath, parameters);
        }

        private string GetParameterValue(string name, Parameter[] parameters)
        {
            var parameter = GetRouteParameter(name, parameters);
            if (!parameter.Value!.GetType().IsPrimitive())
                throw _clientValidationExceptionFactory.TemplatePartContainsComplexType(name);

            return parameter.Value.ToString() ?? "";
        }

        private string GetCustomParameterValue(string objectName, string memberPath, Parameter[] parameters)
        {
            var parameter = GetRouteParameter(objectName, parameters);

            return (parameter.Attribute switch
            {
                BodyParamAttribute => _objectMemberManager.GetValue(parameter.Value!, memberPath, new BodyMemberNameSelector()),
                QueryParamAttribute => _objectMemberManager.GetValue(parameter.Value!, memberPath, new QueryMemberNameSelector()),
                { } => _objectMemberManager.GetValue(parameter.Value!, memberPath, new DefaultMemberNameSelector()),
                _ => throw new ArgumentNullException($"Parameter '{parameter.Name}' has no attribute.")
            })?.ToString() ?? "";
        }

        private Parameter GetRouteParameter(string name, IEnumerable<Parameter> parameters)
        {
            var parameterValue = parameters.SingleOrDefault(x => x.Name == name);
            if (parameterValue is null)
                throw _clientValidationExceptionFactory.TokenNotMatchAnyMethodParameter(name);
            if (parameterValue.Value is null)
                throw _clientArgumentExceptionFactory.ParameterInRouteTemplateIsNull(name);
            return parameterValue!;
        }

        private string GetValueForText(TemplatePart templatePart, string clientName, string methodName)
        {
            return templatePart.Text switch
            {
                "[controller]" => GetControllerName(clientName),
                "[action]" => methodName,
                { Length: > 2 } token when token.First() == '[' && token.Last() == ']' =>
                    throw _clientValidationExceptionFactory.SpecialTokenFromTemplateNotExists(token),
                _ => templatePart.Text ?? throw new ArgumentException($"Token '{nameof(templatePart.Text)}' from template '{templatePart}' is null.", nameof(templatePart))
            };
        }

        private string GetControllerName(string name)
        {
            var nameWithoutSuffixAndPrefix = GetNameWithoutSuffix(GetNameWithoutPrefix(name));
            if (string.IsNullOrEmpty(nameWithoutSuffixAndPrefix))
                throw _clientValidationExceptionFactory.ClientNameConsistsOnlyOfSuffixesAndPrefixes();
            return nameWithoutSuffixAndPrefix;
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
