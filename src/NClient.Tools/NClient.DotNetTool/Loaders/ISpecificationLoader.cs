using System.Threading.Tasks;

namespace NClient.DotNetTool.Loaders
{
    public interface ISpecificationLoader
    {
        Task<string> LoadAsync();
    }
}