namespace NClient.DotNetTool.Loaders
{
    public interface ILoaderFactory
    {
        ISpecificationLoader Create(CommandLineOptions opts);
    }
}