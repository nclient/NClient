using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSwag;
using NSwag.CodeGeneration.CSharp;

namespace NClient.Providers.CodeGeneration.NSwag
{
    public class NSwagGenerator : INClientGenerator
    {
        private readonly ILogger? _logger;
        public NSwagGenerator(ILogger? logger)
        {
            _logger = logger;
        }
        public async Task<string?> GenerateAsync(string specification, string @namespace, CancellationToken cancellationToken = default)
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
            
            settings.CSharpGeneratorSettings.TemplateFactory = new DefaultTemplateFactory(settings.CSharpGeneratorSettings, new[]
            {
                typeof(NSwagGenerator).GetTypeInfo().Assembly,
                typeof(NJsonSchema.CodeGeneration.CSharp.CSharpGenerator).GetTypeInfo().Assembly
            });
            
            var generator = new CSharpInterfaceGenerator(openApiDocument, settings, _logger);
            return generator.GenerateFile();
        }
    }
}