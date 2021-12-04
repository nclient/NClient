using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

namespace NClient.CodeGeneration.Generator.Models
{
    /// <summary>The CSharp controller operation model.</summary>
    internal class CSharpInterfaceOperationModel : CSharpOperationModel
    {
        private readonly CSharpControllerGeneratorSettings _settings;

        /// <summary>Initializes a new instance of the <see cref="CSharpInterfaceOperationModel" /> class.</summary>
        /// <param name="operation">The operation.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="generator">The generator.</param>
        /// <param name="resolver">The resolver.</param>
        public CSharpInterfaceOperationModel(OpenApiOperation operation, CSharpControllerGeneratorSettings settings,
            CSharpInterfaceGenerator generator, CSharpTypeResolver resolver)
            : base(operation, settings, generator, resolver)
        {
            _settings = settings;
        }

        /// <summary>Gets or sets the type of the result.</summary>
        public override string ResultType
        {
            get
            {
                if (_settings.UseActionResultType)
                    switch (SyncResultType)
                    {
                        case "void":
                        case "FileResult":
                            return "System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult>";
                        default:
                            return "System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<" + SyncResultType + ">>";
                    }

                return base.ResultType;
            }
        }
    }
}
