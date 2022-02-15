using NClient.CodeGeneration.Abstractions.Enums;

namespace NClient.CodeGeneration.Abstractions
{
    public class FacadeGenerationSettings
    {
        public string Name { get; }
        public string Namespace { get; }
        public bool GenerateClients { get; }
        public bool GenerateFacades { get; }
        public bool UseModelValidationAttributes { get; }
        public bool UseNullableReferenceTypes { get; }
        public bool UseCancellationToken { get; }
        public bool UseDtoTypes { get; }
        public SerializeType SerializeType { get; }
        
        public FacadeGenerationSettings(
            string name,
            string @namespace, 
            bool generateClients,
            bool generateFacades,
            bool useModelValidationAttributes, 
            bool useNullableReferenceTypes, 
            bool useCancellationToken, 
            bool useDtoTypes,
            SerializeType serializeType)
        {
            Name = name;
            Namespace = @namespace;
            GenerateClients = generateClients;
            GenerateFacades = generateFacades;
            UseModelValidationAttributes = useModelValidationAttributes;
            UseNullableReferenceTypes = useNullableReferenceTypes;
            UseCancellationToken = useCancellationToken;
            UseDtoTypes = useDtoTypes;
            SerializeType = serializeType;
        }
    }
}
