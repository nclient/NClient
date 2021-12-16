using NClient.DotNetTool.Options;

namespace NClient.DotNetTool.Loaders
{
    public interface ILoaderFactory
    {
        ISpecificationLoader Create(FacadeOptions.GenerationOptions generationOptions);
    }
}