using System.Threading.Tasks;

namespace NClient.DotNetTool.Savers
{
    public interface ISaver
    {
        Task SaveAsync(string content, string output);
    }
}
