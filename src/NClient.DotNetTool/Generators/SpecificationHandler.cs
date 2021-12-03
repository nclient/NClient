using System.Threading;
using System.Threading.Tasks;
using NSwag.CodeGeneration.CSharp;

namespace NClient.DotNetTool.Generators
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
            
            var generator = new CSharpInterfaceGenerator(openApiDocument, settings);
            return generator.GenerateFile();
        }
    }
}