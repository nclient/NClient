using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

namespace NClient.CodeGeneration.Providers.NSwag.Models
{
    internal class CSharpInterfaceOperationModel : CSharpControllerOperationModel
    {
        private readonly CSharpControllerGeneratorSettings _settings;
        
        public CSharpInterfaceOperationModel(OpenApiOperation operation, CSharpControllerGeneratorSettings settings,
            CSharpInterfaceGenerator generator, CSharpTypeResolver resolver)
            : base(operation, settings, generator, resolver)
        {
            _settings = settings;
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
