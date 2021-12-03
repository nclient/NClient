using System.Collections.Generic;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

namespace NClient.DotNetTool.Generators
{
    public class CSharpInterfaceGenerator : CSharpClientGenerator
    {
        private readonly NSwag.OpenApiDocument _document;
        
        public CSharpInterfaceGenerator(NSwag.OpenApiDocument document, CSharpClientGeneratorSettings settings) : base(document, settings)
        {
            _document = document;
        }

        /// <summary>Generates the the whole file containing all needed types.</summary>
        /// <returns>The code</returns>
        /// <footer><a href="https://www.google.com/search?q=NSwag.CodeGeneration.ClientGeneratorBase%603.GenerateFile">`ClientGeneratorBase.GenerateFile` on google.com</a></footer>
        public new string? GenerateFile()
        {
            return base.GenerateFile();
        }
        
        /// <summary>Generates the client class.</summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="controllerClassName">Name of the controller class.</param>
        /// <param name="operations">The operations.</param>
        /// <returns>The code.</returns>
        protected override IEnumerable<CodeArtifact> GenerateClientTypes(string controllerName, string controllerClassName, IEnumerable<CSharpOperationModel> operations)
        {
            var exceptionSchema = (Resolver as CSharpTypeResolver)?.ExceptionSchema;

            var model = new CSharpClientTemplateModel(controllerName, controllerClassName, operations, exceptionSchema, _document, Settings);
            if (!model.HasOperations)
                yield break;
            if (!model.GenerateClientInterfaces)
                yield break;
            var interfaceTemplate = Settings.CSharpGeneratorSettings.TemplateFactory.CreateTemplate("CSharp", "Client.Interface", model);
            yield return new CodeArtifact(model.Class, CodeArtifactType.Interface, CodeArtifactLanguage.CSharp, CodeArtifactCategory.Contract, interfaceTemplate);
        }
    }
}
