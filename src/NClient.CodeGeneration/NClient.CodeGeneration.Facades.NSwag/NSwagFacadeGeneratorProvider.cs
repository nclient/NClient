using Microsoft.Extensions.Logging;
using NClient.CodeGeneration.Abstractions;

namespace NClient.CodeGeneration.Facades.NSwag
{
    public class NSwagFacadeGeneratorProvider : INClientFacadeGeneratorProvider
    {
        public INClientFacadeGenerator Create(ILogger? logger)
        {
            return new NSwagFacadeGenerator(logger);
        }
    }
}
