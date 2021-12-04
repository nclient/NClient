using Microsoft.Extensions.Logging;

namespace NClient.Providers.CodeGeneration.NSwag
{
    public class NSwagGeneratorProvider : INClientGeneratorProvider
    {
        public INClientGenerator Create(ILogger? logger)
        {
            return new NSwagGenerator(logger);
        }
    }
}
