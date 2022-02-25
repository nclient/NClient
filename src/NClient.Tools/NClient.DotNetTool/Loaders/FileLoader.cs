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
        
        public Task<string> LoadAsync()
        {
            #if NETFRAMEWORK
            return Task.FromResult(File.ReadAllText(_path));
            #else
            return File.ReadAllTextAsync(_path);
            #endif
        }
    }
}