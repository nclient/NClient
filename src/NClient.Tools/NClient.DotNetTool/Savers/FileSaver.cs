using System.IO;
using System.Threading.Tasks;

namespace NClient.DotNetTool.Savers
{
    public class FileSaver : ISaver
    {
        public async Task SaveAsync(string content, string output)
        {
            if (File.Exists(output))
                File.Delete(output);

            #if NETFRAMEWORK
            File.WriteAllText(output, content);
            await Task.CompletedTask.ConfigureAwait(false);
            #else
            await File.WriteAllTextAsync(output, content);
            #endif
        }
    }
}
