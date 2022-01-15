using System.Collections.Generic;
using System.Linq;
using NSwag;
using NSwag.CodeGeneration.CSharp.Models;

namespace NClient.CodeGeneration.Facades.NSwag.Models
{
    internal class CSharpFacadeTemplateModel : CSharpTemplateModelBase
    {
        private readonly CSharpFacadeGeneratorSettings _settings;
        private readonly OpenApiDocument _document;
        
        public CSharpFacadeTemplateModel(
            string facadeName,
            IEnumerable<CSharpOperationModel> operations,
            OpenApiDocument document,
            CSharpFacadeGeneratorSettings settings)
            : base(facadeName, settings)
        {
            _document = document;
            _settings = settings;

            FacadeName = facadeName;
            Operations = operations;
        }

        public bool GenerateClients => _settings.GenerateClients;
        
        public bool GenerateFacades => _settings.GenerateFacades;
        
        public string FacadeName { get; }
        
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

        public bool UseCancellationToken => _settings.UseCancellationToken;

        public bool GenerateModelValidationAttributes => _settings.GenerateModelValidationAttributes;

        public string RequiredAttributeType => IsAspNetCore ? "Microsoft.AspNetCore.Mvc.ModelBinding.BindRequired" : "System.ComponentModel.DataAnnotations.Required";

        public string Title => _document.Info.Title;

        public string Description => _document.Info.Description;

        public string Version => _document.Info.Version;
    }
}
