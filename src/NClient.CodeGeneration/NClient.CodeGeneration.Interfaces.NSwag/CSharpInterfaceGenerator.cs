using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using NClient.CodeGeneration.Interfaces.NSwag.Models;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

namespace NClient.CodeGeneration.Interfaces.NSwag
{
    internal class CSharpInterfaceGenerator : CSharpControllerGenerator
    {
        private readonly OpenApiDocument _document;
        private readonly ILogger? _logger;
        
        public CSharpInterfaceGenerator(OpenApiDocument document, CSharpControllerGeneratorSettings settings, ILogger? logger)
            : base(document, settings, CreateResolverWithExceptionSchema(settings.CSharpGeneratorSettings, document))
        {
            _document = document;
            _logger = logger;
        }
            
        protected override IEnumerable<CodeArtifact> GenerateClientTypes(string interfaceName, string interfaceDefinitionName, IEnumerable<CSharpOperationModel> operations)
        {
            var allOperations = operations.ToArray();
            var availableOperations = allOperations.Where(o => !o.Consumes.Contains("multipart"));
            var notAvailableOperations = allOperations.Where(o => o.Consumes.Contains("multipart"));
            foreach (var notAvailableOperation in notAvailableOperations)
            {
                _logger?.LogWarning($"Multipart content currently not supported. Operation {notAvailableOperation.Summary ?? notAvailableOperation.ActualOperationName} was skipped!");
            }
            var model = new CSharpInterfaceTemplateModel(interfaceDefinitionName, availableOperations, _document, Settings);
            var template = Settings.CodeGeneratorSettings.TemplateFactory.CreateTemplate("CSharp", "Interface", model);
            yield return new CodeArtifact(interfaceName, CodeArtifactType.Class, CodeArtifactLanguage.CSharp, CodeArtifactCategory.Client, template);
        }
        
        protected override CSharpOperationModel CreateOperationModel(OpenApiOperation operation, ClientGeneratorBaseSettings settings) => 
            new CSharpInterfaceOperationModel(operation, (CSharpControllerGeneratorSettings) settings, this, (CSharpTypeResolver) Resolver);
    }
}
