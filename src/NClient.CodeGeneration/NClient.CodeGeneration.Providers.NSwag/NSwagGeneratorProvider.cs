using Microsoft.Extensions.Logging;
using NClient.CodeGeneration.Abstractions;

namespace NClient.CodeGeneration.Providers.NSwag
{
    public class NSwagGeneratorProvider : INClientGeneratorProvider
    {
        public INClientGenerator Create(ILogger? logger)
        {
            return new NSwagGenerator(logger);
        }
    }
}
