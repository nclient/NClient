using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NClient.CodeGeneration.Abstractions;
using NClient.CodeGeneration.Abstractions.Enums;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration.OperationNameGenerators;

namespace NClient.CodeGeneration.Facades.NSwag
{
    public class NSwagFacadeGenerator : INClientFacadeGenerator
    {
        private readonly ILogger? _logger;
        
        public NSwagFacadeGenerator(ILogger? logger)
        {
            _logger = logger;
        }
        
        public async Task<string> GenerateAsync(string specification, FacadeGenerationSettings generationSettings, CancellationToken cancellationToken = default)
        {
            var openApiDocument = await OpenApiDocument.FromJsonAsync(specification, cancellationToken);
            
            var settings = new CSharpFacadeGeneratorSettings
            {
                GenerateClientInterfaces = true,
                GenerateResponseClasses = false,
                OperationNameGenerator = new MultipleClientsFromFirstTagAndOperationIdGenerator(),
                
                GenerateClients = generationSettings.GenerateClients,
                GenerateFacades = generationSettings.GenerateFacades,
                
                ClassName = generationSettings.Name
                    .Replace("{facade}", "{controller}")
                    .Replace("{client}", "{controller}"),
                GenerateModelValidationAttributes = generationSettings.UseModelValidationAttributes,
                GenerateDtoTypes = generationSettings.UseDtoTypes,
                UseCancellationToken = generationSettings.UseCancellationToken,
                CSharpGeneratorSettings = 
                {
                    Namespace = generationSettings.Namespace,
                    GenerateNullableReferenceTypes = generationSettings.UseNullableReferenceTypes,
                    JsonLibrary = generationSettings.SerializeType switch
                    {
                        SerializeType.SystemJsonText => CSharpJsonLibrary.SystemTextJson,
                        SerializeType.NewtonsoftJson => CSharpJsonLibrary.NewtonsoftJson,
                        { } => throw new NotSupportedException($"Serializer type '{generationSettings.SerializeType}' not supported.")
                    }
                }
            };
            
            settings.CSharpGeneratorSettings.TemplateFactory = new FacadeTemplateFactory(settings.CSharpGeneratorSettings, new[]
            {
                typeof(NSwagFacadeGenerator).GetTypeInfo().Assembly,
                typeof(CSharpGenerator).GetTypeInfo().Assembly
            });
 
            var generator = new CSharpFacadeGenerator(openApiDocument, settings, _logger);
            return generator.GenerateFile();
        }
    }
}