using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NClient.Annotations.CodeGeneration;
using NSwag;
using NSwag.CodeGeneration.CSharp;

namespace NClient.Providers.CodeGeneration.NSwag
{
    public class NSwagGeneratorProvider : INClientGenerator
    {
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
                typeof(NSwagGeneratorProvider).GetTypeInfo().Assembly,
                typeof(NJsonSchema.CodeGeneration.CSharp.CSharpGenerator).GetTypeInfo().Assembly
            });
            
            var generator = new CSharpInterfaceGenerator(openApiDocument, settings);
            return generator.GenerateFile();
        }
    }
}