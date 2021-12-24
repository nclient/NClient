using NSwag.CodeGeneration.CSharp;

namespace NClient.CodeGeneration.Facades.NSwag
{
    public class CSharpFacadeGeneratorSettings : CSharpControllerGeneratorSettings
    {
        public bool GenerateClients { get; set; }
        public bool GenerateFacades { get; set; }
    }
}
