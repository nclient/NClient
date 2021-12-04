using System.Collections.Generic;
using NClient.CodeGeneration.Providers.NSwag.Models;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

namespace NClient.CodeGeneration.Providers.NSwag
{
    internal class CSharpInterfaceGenerator : CSharpControllerGenerator
    {
        private readonly OpenApiDocument _document;
        
        public CSharpInterfaceGenerator(OpenApiDocument document, CSharpControllerGeneratorSettings settings)
            : base(document, settings, CreateResolverWithExceptionSchema(settings.CSharpGeneratorSettings, document))
        {
            _document = document;
        }
            
        protected override IEnumerable<CodeArtifact> GenerateClientTypes(string interfaceName, string interfaceDefinitionName, IEnumerable<CSharpOperationModel> operations)
        {
            var model = new CSharpInterfaceTemplateModel(interfaceName, operations, _document, Settings);
            var template = Settings.CodeGeneratorSettings.TemplateFactory.CreateTemplate("CSharp", "Controller", model);
            yield return new CodeArtifact(model.InterfaceName, CodeArtifactType.Class, CodeArtifactLanguage.CSharp, CodeArtifactCategory.Client, template);
        }
        
        protected override CSharpOperationModel CreateOperationModel(OpenApiOperation operation, ClientGeneratorBaseSettings settings) => 
            new CSharpInterfaceOperationModel(operation, (CSharpControllerGeneratorSettings) settings, this, (CSharpTypeResolver) Resolver);
    }
}
