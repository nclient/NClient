using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

namespace NClient.CodeGeneration.Interfaces.NSwag.Models
{
    internal class CSharpFacadeOperationModel : CSharpControllerOperationModel
    {
        private readonly CSharpControllerGeneratorSettings _settings;
        
        public CSharpFacadeOperationModel(OpenApiOperation operation, CSharpControllerGeneratorSettings settings,
            CSharpFacadeGenerator generator, CSharpTypeResolver resolver)
            : base(operation, settings, generator, resolver)
        {
            _settings = settings;
        }
        
        /// <summary>Gets or sets the type of the result.</summary>
        public string ClientResultType
        {
            get
            {
                switch (SyncResultType)
                {
                    case "void":
                    case "FileResult":
                        return "System.Threading.Tasks.Task<NClient.Providers.Results.HttpResults.IHttpResponse>";
                    default:
                        return "System.Threading.Tasks.Task<NClient.Providers.Results.HttpResults.IHttpResponse<" + SyncResultType + ">>";
                }
            }
        }

        protected override string ResolveParameterType(OpenApiParameter parameter)
        {
            var schema = parameter.ActualSchema;

            if (parameter.IsBinaryBodyParameter)
            {
                if (schema.Type == JsonObjectType.Array && schema.Item.IsBinary)
                    return "System.Collections.Generic.IEnumerable<byte[]>";
                
                return "byte[]";
            }

            if (schema.Type == JsonObjectType.Array && schema.Item.IsBinary)
                return "System.Collections.Generic.IEnumerable<byte[]>";

            if (schema.IsBinary)
            {
                if (parameter.CollectionFormat == OpenApiParameterCollectionFormat.Multi && !schema.Type.HasFlag(JsonObjectType.Array))
                    return "System.Collections.Generic.IEnumerable<byte[]>";

                return "byte[]";
            }

            return base.ResolveParameterType(parameter);
        }
    }
}
