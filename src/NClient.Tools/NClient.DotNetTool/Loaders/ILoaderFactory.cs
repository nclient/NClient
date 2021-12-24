using NClient.DotNetTool.Options;

namespace NClient.DotNetTool.Loaders
{
    public interface ILoaderFactory
    {
        ISpecificationLoader Create(InterfaceGenerationOptions generationOptions);
    }
}