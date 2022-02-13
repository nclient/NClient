using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using NClient.CodeGeneration.Facades.NSwag.Models;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

namespace NClient.CodeGeneration.Facades.NSwag
{
    internal class CSharpFacadeGenerator : CSharpControllerGenerator
    {
        private readonly OpenApiDocument _document;
        private readonly ILogger? _logger;
        
        public CSharpFacadeGenerator(OpenApiDocument document, CSharpFacadeGeneratorSettings settings, ILogger? logger)
            : base(document, settings, CreateResolverWithExceptionSchema(settings.CSharpGeneratorSettings, document))
        {
            _document = document;
            _logger = logger;
        }
            
        protected override IEnumerable<CodeArtifact> GenerateClientTypes(string facadeName, string facadeDefinitionName, IEnumerable<CSharpOperationModel> operations)
        {
            var allOperations = operations.ToArray();
            var availableOperations = allOperations.Where(o => !o.Consumes.Contains("multipart"));
            var notAvailableOperations = allOperations.Where(o => o.Consumes.Contains("multipart"));
            foreach (var notAvailableOperation in notAvailableOperations)
            {
                var operationName = notAvailableOperation.Summary ?? notAvailableOperation.ActualOperationName;
                _logger?.LogWarning("Multipart content currently not supported: operation {OperationName} was skipped", operationName);
            }
            var model = new CSharpFacadeTemplateModel(facadeDefinitionName, availableOperations, _document, (CSharpFacadeGeneratorSettings) Settings);
            var template = Settings.CodeGeneratorSettings.TemplateFactory.CreateTemplate("CSharp", "Facade", model);
            yield return new CodeArtifact(facadeName, CodeArtifactType.Class, CodeArtifactLanguage.CSharp, CodeArtifactCategory.Client, template);
        }
        
        protected override CSharpOperationModel CreateOperationModel(OpenApiOperation operation, ClientGeneratorBaseSettings settings) => 
            new CSharpFacadeOperationModel(operation, (CSharpControllerGeneratorSettings) settings, this, (CSharpTypeResolver) Resolver);
    }
}
