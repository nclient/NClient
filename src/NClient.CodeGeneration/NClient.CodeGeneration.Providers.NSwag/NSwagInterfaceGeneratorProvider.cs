using Microsoft.Extensions.Logging;
using NClient.CodeGeneration.Abstractions;

namespace NClient.CodeGeneration.Providers.NSwag
{
    public class NSwagInterfaceGeneratorProvider : INClientInterfaceGeneratorProvider
    {
        public INClientInterfaceGenerator Create(ILogger? logger)
        {
            return new NSwagInterfaceGenerator(logger);
        }
    }
}
