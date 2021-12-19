using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NClient.CodeGeneration.Abstractions;
using NClient.CodeGeneration.Abstractions.Enums;
using NClient.DotNetTool.Options;

namespace NClient.DotNetTool
{
    public interface IFacadeGenerator
    {
        Task<string> GenerateAsync(FacadeOptions.GenerationOptions generationOptions, string specification);
    }
    
    public class FacadeGenerator : IFacadeGenerator
    {
        private readonly ILogger<FacadeGenerator> _logger;
        private readonly INClientFacadeGenerator _facadeGenerator;
        
        public FacadeGenerator(INClientFacadeGenerator facadeGenerator, ILogger<FacadeGenerator> logger)
        {
            _logger = logger;
            _facadeGenerator = facadeGenerator;
        }

        public async Task<string> GenerateAsync(FacadeOptions.GenerationOptions generationOptions, string specification)
        {
            var generationSettings = new FacadeGenerationSettings(
                generationOptions.FacadeName, 
                generationOptions.Namespace,
                generationOptions.UseModelValidationAttributes,
                generationOptions.UseNullableReferenceTypes,
                generationOptions.UseCancellationToken,
                generationOptions.GenerateDtoTypes,
                generationOptions.UseSystemTextJson 
                    ? SerializeType.SystemJsonText 
                    : SerializeType.NewtonsoftJson);
            return await _facadeGenerator.GenerateAsync(specification, generationSettings);
        }
    }
}
