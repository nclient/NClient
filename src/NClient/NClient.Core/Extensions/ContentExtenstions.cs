using System.IO;
using System.Threading.Tasks;
using NClient.Providers.Transport;

namespace NClient.Core.Extensions
{
    internal static class ContentExtensions
    {
        internal static async Task<string> ReadToEndAsync(this IContent content)
        {
            return await new StreamReader(content.Stream).ReadToEndAsync().ConfigureAwait(false);    
        }
    }
}
