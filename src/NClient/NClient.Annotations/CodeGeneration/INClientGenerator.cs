using System.Threading;
using System.Threading.Tasks;

namespace NClient.Annotations.CodeGeneration
{
    public interface INClientGenerator
    {
        Task<string?> GenerateAsync(string specification, string @namespace, CancellationToken cancellationToken = default);
    }
}
