using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NClient.CodeGeneration.Abstractions;
using NSwag;
using NSwag.CodeGeneration.CSharp;

namespace NClient.CodeGeneration.Providers.NSwag
{
    public class NSwagInterfaceGenerator : INClientInterfaceGenerator
    {
        private readonly ILogger? _logger;
        
        public NSwagInterfaceGenerator(ILogger? logger)
        {
            _logger = logger;
        }
        
        public async Task<string> GenerateAsync(string specification, string @namespace, CancellationToken cancellationToken = default)
        {
            var openApiDocument = await OpenApiDocument.FromJsonAsync(specification, cancellationToken);
            
            var settings = new CSharpControllerGeneratorSettings
            {
                GenerateClientInterfaces = true,
                UseCancellationToken = true,
                CSharpGeneratorSettings = 
                {
                    Namespace = @namespace
                },
                AdditionalNamespaceUsages = new[]
                {
                    "NClient.Annotations",
                    "NClient.Annotations.Http"
                }
            };
            
            settings.CSharpGeneratorSettings.TemplateFactory = new InterfaceTemplateFactory(settings.CSharpGeneratorSettings, new[]
            {
                typeof(NSwagInterfaceGenerator).GetTypeInfo().Assembly,
                typeof(NJsonSchema.CodeGeneration.CSharp.CSharpGenerator).GetTypeInfo().Assembly
            });
            
            var generator = new CSharpInterfaceGenerator(openApiDocument, settings, _logger);
            return generator.GenerateFile();
        }
    }
}