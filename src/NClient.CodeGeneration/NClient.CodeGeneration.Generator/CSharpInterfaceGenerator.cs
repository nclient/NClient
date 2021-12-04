using System;
using System.Collections.Generic;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

namespace NClient.CodeGeneration.Generator
{
  public class CSharpInterfaceGenerator : CSharpClientGenerator
  {
    private readonly OpenApiDocument _document;
    public CSharpInterfaceGenerator(OpenApiDocument document, CSharpClientGeneratorSettings settings) : base(document, settings)
    {
      _document = document;
    }
        
    /// <summary>Generates the client class.</summary>
    /// <param name="controllerName">Name of the controller.</param>
    /// <param name="controllerClassName">Name of the controller class.</param>
    /// <param name="operations">The operations.</param>
    /// <returns>The code.</returns>
    /// <footer><a href="https://www.google.com/search?q=NSwag.CodeGeneration.CSharp.CSharpClientGenerator.GenerateClientTypes">`CSharpClientGenerator.GenerateClientTypes` on google.com</a></footer>
    protected override IEnumerable<CodeArtifact> GenerateClientTypes(
      string controllerName,
      string controllerClassName,
      IEnumerable<CSharpOperationModel> operations)
    {
      var csharpClientGenerator = this;
      var model = new CSharpClientTemplateModel(controllerName, controllerClassName, operations, csharpClientGenerator.Resolver is CSharpTypeResolver resolver ? resolver.ExceptionSchema : null, csharpClientGenerator._document, csharpClientGenerator.Settings);
      if (!model.HasOperations)
        yield break;
      if (model.GenerateClientInterfaces)
        yield return new CodeArtifact(model.Class, CodeArtifactType.Interface, CodeArtifactLanguage.CSharp, CodeArtifactCategory.Contract, csharpClientGenerator.Settings.CSharpGeneratorSettings.TemplateFactory.CreateTemplate("CSharp", "Client.Interface", model));
      else
        throw new ArgumentOutOfRangeException();
    }

    /// <summary>Creates an operation model.</summary>
    /// <param name="operation">The operation.</param>
    /// <param name="settings">The settings.</param>
    /// <returns>The operation model.</returns>
    /// <footer><a href="https://www.google.com/search?q=NSwag.CodeGeneration.CSharp.CSharpClientGenerator.CreateOperationModel">`CSharpClientGenerator.CreateOperationModel` on google.com</a></footer>
    protected override CSharpOperationModel CreateOperationModel(
      OpenApiOperation operation,
      ClientGeneratorBaseSettings settings)
    {
      return new CSharpOperationModel(operation, (CSharpGeneratorBaseSettings) settings, this, (CSharpTypeResolver) Resolver);
    }
  }
}
