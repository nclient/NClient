using System.IO;
using System.Threading.Tasks;

namespace NClient.DotNetTool.Loaders
{
    public class FileLoader : ISpecificationLoader
    {
        private readonly string _path;
        public FileLoader(string path)
        {
            _path = path;
        }
        
        public Task<string> Load()
        {
            return File.ReadAllTextAsync(_path);
        }
    }
}