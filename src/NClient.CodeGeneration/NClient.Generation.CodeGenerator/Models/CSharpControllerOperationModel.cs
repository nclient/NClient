//-----------------------------------------------------------------------
// <copyright file="CSharpControllerOperationModel.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

namespace NClient.Generation.CodeGenerator.Models
{
    /// <summary>The CSharp controller operation model.</summary>
    public class CSharpControllerOperationModel : CSharpOperationModel
    {
        private readonly CSharpControllerGeneratorSettings _settings;

        /// <summary>Initializes a new instance of the <see cref="CSharpControllerOperationModel" /> class.</summary>
        /// <param name="operation">The operation.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="generator">The generator.</param>
        /// <param name="resolver">The resolver.</param>
        public CSharpControllerOperationModel(OpenApiOperation operation, CSharpControllerGeneratorSettings settings,
            CSharpControllerGenerator generator, CSharpTypeResolver resolver)
            : base(operation, settings, generator, resolver)
        {
            _settings = settings;
        }

        /// <summary>Gets or sets the type of the result.</summary>
        public override string ResultType
        {
            get
            {
                if (!_settings.UseActionResultType)
                    return base.ResultType;

                return SyncResultType switch
                {
                    "void" => "System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult>",
                    "FileResult" => "System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult>",
                    _ => "System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<" + SyncResultType + ">>"
                };
            }
        }
    }
}
