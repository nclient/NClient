using System.Collections.Generic;
using System.Linq;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

namespace NClient.CodeGeneration.Providers.NSwag.Models
{
    internal class CSharpInterfaceTemplateModel : CSharpTemplateModelBase
    {
        private readonly CSharpControllerGeneratorSettings _settings;
        private readonly OpenApiDocument _document;
        
        public CSharpInterfaceTemplateModel(
            string interfaceName,
            IEnumerable<CSharpOperationModel> operations,
            OpenApiDocument document,
            CSharpControllerGeneratorSettings settings)
            : base(interfaceName, settings)
        {
            _document = document;
            _settings = settings;

            InterfaceName = interfaceName;
            Operations = operations;
        }
        
        public string InterfaceName { get; }
        
        public string NClientAnnotationsNamespace => "NClient.Annotations";
        
        public string NClientAnnotationsHttpNamespace => "NClient.Annotations.Http";

        public bool IsAspNetCore => _settings.ControllerTarget == CSharpControllerTarget.AspNetCore;

        public bool IsAspNet => _settings.ControllerTarget == CSharpControllerTarget.AspNet;

        public string BaseUrl => _document.BaseUrl;

        public bool HasOperations => Operations.Any();

        public IEnumerable<CSharpOperationModel> Operations { get; set; }

        public bool HasBasePath => !string.IsNullOrEmpty(BasePath);

        public string? BasePath => string.IsNullOrEmpty(_settings.BasePath) ? _document.BasePath?.TrimStart('/') : _settings.BasePath.TrimStart('/');

        public bool GenerateOptionalParameters => _settings.GenerateOptionalParameters;

        public bool GeneratePartialInterfaces => _settings.ControllerStyle == CSharpControllerStyle.Partial;
        
        public bool UseCancellationToken => _settings.UseCancellationToken;

        public bool GenerateModelValidationAttributes => _settings.GenerateModelValidationAttributes;

        public string RequiredAttributeType => IsAspNetCore ? "Microsoft.AspNetCore.Mvc.ModelBinding.BindRequired" : "System.ComponentModel.DataAnnotations.Required";

        public string Title => _document.Info.Title;

        public string Description => _document.Info.Description;

        public string Version => _document.Info.Version;
    }
}
