using System.Threading;
using System.Threading.Tasks;

namespace NClient.Providers.CodeGeneration
{
    public interface INClientGenerator
    {
        Task<string?> GenerateAsync(string specification, string @namespace, CancellationToken cancellationToken = default);
    }
}
