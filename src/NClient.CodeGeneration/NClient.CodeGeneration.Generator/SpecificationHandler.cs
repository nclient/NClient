using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NSwag.CodeGeneration.CSharp;

namespace NClient.CodeGeneration.Generator
{
    public interface ISpecificationHandler
    {
        Task<string?> Generate(string specification, string @namespace, CancellationToken cancellationToken = default);
    }
    
    public class SpecificationHandler : ISpecificationHandler
    {
        public async Task<string?> Generate(string specification, string @namespace, CancellationToken cancellationToken = default)
        {
            var openApiDocument = await NSwag.OpenApiDocument.FromJsonAsync(specification, cancellationToken);
            
            var settings = new CSharpClientGeneratorSettings
            {
                GenerateClientInterfaces = true,
                CSharpGeneratorSettings = 
                {
                    Namespace = @namespace
                }
            };
            
            settings.CSharpGeneratorSettings.TemplateFactory = new CustomTemplateFactory(settings.CSharpGeneratorSettings, new[]
            {
                typeof(SpecificationHandler).GetTypeInfo().Assembly,
                typeof(NJsonSchema.CodeGeneration.CSharp.CSharpGenerator).GetTypeInfo().Assembly
            });
            
            var generator = new CSharpInterfaceGenerator(openApiDocument, settings);
            return generator.GenerateFile();
        }
    }
}