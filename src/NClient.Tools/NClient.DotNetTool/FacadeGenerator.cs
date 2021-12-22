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
        Task<string> GenerateAsync(CommonGenerationOptions generationOptions, string specification);
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

        public Task<string> GenerateAsync(CommonGenerationOptions generationOptions, string specification)
        {
            var generationSettings = new FacadeGenerationSettings(
                name: (generationOptions as ClientOptions.GenerationOptions)?.ClientName 
                ?? (generationOptions as FacadeOptions.GenerationOptions)?.FacadeName
                ?? "{controller}", 
                generationOptions.Namespace,
                generateClients: (generationOptions as FacadeOptions.GenerationOptions)?.GenerateClients ?? false,
                generateFacades: (generationOptions as ClientOptions.GenerationOptions)?.GenerateFacades ?? false,
                generationOptions.UseModelValidationAttributes,
                generationOptions.UseNullableReferenceTypes,
                generationOptions.UseCancellationToken,
                generationOptions.GenerateDtoTypes,
                GetSerializerType(generationOptions.UseSystemTextJson, generationOptions.UseNewtonsoftJson));
            return _facadeGenerator.GenerateAsync(specification, generationSettings);
        }

        private static SerializeType GetSerializerType(bool useSystemTextJson, bool useNewtonsoftJson)
        {
            return (UseSystemTextJson: useSystemTextJson, UseNewtonsoftJson: useNewtonsoftJson) switch
            {
                { UseSystemTextJson: false, UseNewtonsoftJson: false } => SerializeType.SystemJsonText,
                { UseSystemTextJson: true, UseNewtonsoftJson: false } => SerializeType.SystemJsonText,
                { UseSystemTextJson: false, UseNewtonsoftJson: true } => SerializeType.NewtonsoftJson,
                var x => throw new NotSupportedException($"Not supported serializer configuration: {nameof(x.UseSystemTextJson)} = {x.UseSystemTextJson}; {nameof(x.UseNewtonsoftJson)} = {x.UseNewtonsoftJson}")
            };
        }
    }
}
