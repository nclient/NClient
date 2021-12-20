using System;
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
            var serializerType = (generationOptions.UseSystemTextJson, generationOptions.UseNewtonsoftJson) switch
            {
                { UseSystemTextJson: false, UseNewtonsoftJson: false } => SerializeType.SystemJsonText,
                { UseSystemTextJson: true, UseNewtonsoftJson: false } => SerializeType.SystemJsonText,
                { UseSystemTextJson: false, UseNewtonsoftJson: true } => SerializeType.NewtonsoftJson,
                var x => throw new NotSupportedException($"Not supported serializer configuration: {nameof(x.UseSystemTextJson)} = {x.UseSystemTextJson}; {nameof(x.UseNewtonsoftJson)} = {x.UseNewtonsoftJson}")
            };
            
            var generationSettings = new FacadeGenerationSettings(
                generationOptions.FacadeName, 
                generationOptions.Namespace,
                generationOptions.UseModelValidationAttributes,
                generationOptions.UseNullableReferenceTypes,
                generationOptions.UseCancellationToken,
                generationOptions.GenerateDtoTypes,
                serializerType);
            return await _facadeGenerator.GenerateAsync(specification, generationSettings);
        }
    }
}
