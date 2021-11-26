using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace NClient.DotNetTool
{
    public interface IGenerator
    {
        Dictionary<string, string> Generate(string specification, string @namespace);
    }
    
    public class Generator : IGenerator
    {
        private const string TemplateInterface = @"using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NClient.Annotations;
using NClient.Annotations.Http;

namespace {namespace}
{
    [HttpFacade]
    public interface I{interfaceName}Facade
    {
        {methods}
    }
}
        ";

        private const string TemplateMethod = @"
        [{verb}Method(""{path}"")]        
        Task<IActionResult> {methodName}Async({params});
        ";

        private const string MethodParamTemplate = @"{paramSource} {paramType} {paramName}";

        public Dictionary<string, string> Generate(string specification, string @namespace)
        {
            var openApiDocument = new OpenApiStringReader().Read(specification, out var diagnostic);

            if (diagnostic.Errors.Count > 0)
                throw new AggregateException(diagnostic.Errors.Select(e => new Exception(e.Message)));

            var entities = ParseEntities(openApiDocument);
            
            return GeneratedInterfaces(entities, @namespace);
        }

        private Dictionary<string, string> GeneratedInterfaces(Dictionary<string, List<Tuple<string, OperationType, OpenApiOperation>>> entities, string @namespace)
        {
            var generatedInterfaces = new Dictionary<string, string>();

            foreach (var (entityName, methods) in entities)
            {
                var capitalizedEntityName = string.Concat(entityName[0].ToString().ToUpper(), entityName.AsSpan(1));

                var generatedMethods = new List<string>();

                foreach (var (path, verb, operation) in methods)
                {
                    var generatedParams = new List<string>();

                    foreach (var openApiParameter in operation.Parameters)
                    {
                        var generatedParameter = MethodParamTemplate.Replace(oldValue: "{paramSource}", GetParamSource(openApiParameter))
                            .Replace(oldValue: "{paramType}", GetParamType(openApiParameter))
                            .Replace(oldValue: "{paramName}", openApiParameter.Name);
                        generatedParams.Add(generatedParameter);
                    }

                    var generatedMethod = TemplateMethod.Replace(oldValue: "{verb}", verb.ToString())
                        .Replace(oldValue: "{path}", path)
                        .Replace(oldValue: "{methodName}",
                            string.Concat(operation.OperationId[0].ToString().ToUpper(), operation.OperationId.AsSpan(1)))
                        .Replace(oldValue: "{params}", string.Join(separator: ",", generatedParams));
                    
                    generatedMethods.Add(generatedMethod);
                }

                var generatedInterface = TemplateInterface.Replace(oldValue: "{namespace}", @namespace)
                    .Replace(oldValue: "{interfaceName}", capitalizedEntityName)
                    .Replace(oldValue: "{methods}", string.Join(Environment.NewLine, generatedMethods));

                generatedInterfaces.Add($"I{capitalizedEntityName}Facade", generatedInterface);
            }

            return generatedInterfaces;
        }

        private static string GetParamSource(OpenApiParameter parameter)
        {
            return parameter.In switch
            {
                ParameterLocation.Path => "[RouteParam]",
                ParameterLocation.Header => "[HeaderParam]",
                ParameterLocation.Query => "[QueryParam]",
                _ => "[FromBody]"
            };
        }

        private static string GetParamType(OpenApiParameter parameter)
        {
            if (!string.IsNullOrEmpty(parameter.Schema.Format))
                return string.Concat(parameter.Schema.Format[0].ToString().ToUpper(),
                    parameter.Schema.Format.AsSpan(1));
            
            if (parameter.Schema.Items is null)
                return string.Concat(parameter.Schema.Type[0].ToString().ToUpper(),
                    parameter.Schema.Type.AsSpan(1));
            
            var capitalized = string.Concat(parameter.Schema.Items.Type[0].ToString().ToUpper(),
                parameter.Schema.Items.Type.AsSpan(1));
            return $"{capitalized}[]";
        }

        private static Dictionary<string, List<Tuple<string, OperationType, OpenApiOperation>>> ParseEntities(OpenApiDocument openApiDocument)
        {
            var operations = new Dictionary<string, List<Tuple<string, OperationType, OpenApiOperation>>>();

            foreach (var openApiTag in openApiDocument.Tags)
            {
                foreach (var (path, item) in openApiDocument.Paths)
                {
                    foreach (var (verb, operation) in item.Operations)
                    {
                        if (!operation.Tags.Contains(openApiTag)) continue;
                        
                        if (operations.ContainsKey(openApiTag.Name))
                            operations[openApiTag.Name]
                                .Add(new Tuple<string, OperationType, OpenApiOperation>(path, verb, operation));
                        else
                            operations.Add(openApiTag.Name,
                                new List<Tuple<string, OperationType, OpenApiOperation>>()
                                    {new(path, verb, operation)});
                    }
                }
            }

            return operations;
        }
    }
}