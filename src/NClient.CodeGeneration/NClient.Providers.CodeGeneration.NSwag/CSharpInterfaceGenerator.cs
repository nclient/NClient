using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using NClient.Providers.CodeGeneration.NSwag.Models;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

namespace NClient.Providers.CodeGeneration.NSwag
{
    /// <summary>Generates the CSharp service client code. </summary>
    internal class CSharpInterfaceGenerator : CSharpGeneratorBase
    {
        private readonly OpenApiDocument _document;
        private readonly ILogger? _logger;

        /// <summary>Initializes a new instance of the <see cref="CSharpInterfaceGenerator" /> class.</summary>
        /// <param name="document">The Swagger document.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="logger">The logger (optional)</param>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is <see langword="null" />.</exception>
        public CSharpInterfaceGenerator(OpenApiDocument document, CSharpControllerGeneratorSettings settings, ILogger? logger)
            : this(document, settings, CreateResolverWithExceptionSchema(settings.CSharpGeneratorSettings, document), logger)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CSharpInterfaceGenerator" /> class.</summary>
        /// <param name="document">The Swagger document.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="logger">The logger (optional)</param>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is <see langword="null" />.</exception>
        private CSharpInterfaceGenerator(OpenApiDocument document, CSharpControllerGeneratorSettings settings, CSharpTypeResolver resolver, ILogger? logger)
            : base(document, settings, resolver)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger;
        }

        /// <summary>Gets or sets the generator settings.</summary>
        private CSharpControllerGeneratorSettings Settings { get; set; }

        /// <summary>Gets the base settings.</summary>
        public override ClientGeneratorBaseSettings BaseSettings => Settings;

        /// <summary>Generates the client types.</summary>
        /// <returns>The code artifact collection.</returns>
        protected override IEnumerable<CodeArtifact> GenerateAllClientTypes()
        {
            var artifacts = base.GenerateAllClientTypes().ToList();

            if (Settings.ControllerTarget != CSharpControllerTarget.AspNet || _document.Operations.Count(operation => operation.Operation.ActualParameters.Any(p => p.Kind == OpenApiParameterKind.Header)) <= 0)
                return artifacts;
            
            var template = Settings.CodeGeneratorSettings.TemplateFactory.CreateTemplate("CSharp", "Controller.AspNet.FromHeaderAttribute", new object());
            artifacts.Add(new CodeArtifact("FromHeaderAttribute", CodeArtifactType.Class, CodeArtifactLanguage.CSharp, CodeArtifactCategory.Utility, template));

            template = Settings.CodeGeneratorSettings.TemplateFactory.CreateTemplate("CSharp", "Controller.AspNet.FromHeaderBinding", new object());
            artifacts.Add(new CodeArtifact("FromHeaderBinding", CodeArtifactType.Class, CodeArtifactLanguage.CSharp, CodeArtifactCategory.Utility, template));

            return artifacts;
        }

        /// <summary>Generates the client class.</summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="controllerClassName">Name of the controller class.</param>
        /// <param name="operations">The operations.</param>
        /// <returns>The code.</returns>
        protected override IEnumerable<CodeArtifact> GenerateClientTypes(string controllerName, string controllerClassName, IEnumerable<CSharpOperationModel> operations)
        {
            var model = new Models.CSharpControllerTemplateModel(controllerClassName, operations, _document, Settings);
            var template = Settings.CodeGeneratorSettings.TemplateFactory.CreateTemplate("CSharp", "Controller", model);
            yield return new CodeArtifact(model.Class, CodeArtifactType.Class, CodeArtifactLanguage.CSharp, CodeArtifactCategory.Client, template);
        }

        /// <summary>Creates an operation model.</summary>
        /// <param name="operation">The operation.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The operation model.</returns>
        protected override CSharpOperationModel CreateOperationModel(OpenApiOperation operation, ClientGeneratorBaseSettings settings) => new CSharpInterfaceOperationModel(operation, (CSharpControllerGeneratorSettings) settings, this, (CSharpTypeResolver) Resolver);
    }
}
