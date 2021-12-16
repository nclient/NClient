using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NClient.CodeGeneration.Abstractions;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.OperationNameGenerators;

namespace NClient.CodeGeneration.Facades.NSwag
{
    public class NSwagFacadeGenerator : INClientFacadeGenerator
    {
        private readonly ILogger? _logger;
        
        public NSwagFacadeGenerator(ILogger? logger)
        {
            _logger = logger;
        }
        
        public async Task<string> GenerateAsync(string specification, string @namespace, string facadeName, CancellationToken cancellationToken = default)
        {
            var openApiDocument = await OpenApiDocument.FromJsonAsync(specification, cancellationToken);
            
            var settings = new CSharpControllerGeneratorSettings
            {
                GenerateClientInterfaces = true,
                UseCancellationToken = true,
                CSharpGeneratorSettings = 
                {
                    Namespace = @namespace
                }
            };
            
            settings.CSharpGeneratorSettings.TemplateFactory = new FacadeTemplateFactory(settings.CSharpGeneratorSettings, new[]
            {
                typeof(NSwagFacadeGenerator).GetTypeInfo().Assembly,
                typeof(NJsonSchema.CodeGeneration.CSharp.CSharpGenerator).GetTypeInfo().Assembly
            });

            settings.OperationNameGenerator = new MultipleClientsFromFirstTagAndOperationIdGenerator();
            
            var generator = new CSharpFacadeGenerator(openApiDocument, settings, _logger);
            return generator.GenerateFile();
        }
    }
}