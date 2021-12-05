using System.Threading;
using System.Threading.Tasks;

namespace NClient.CodeGeneration.Abstractions
{
    public interface INClientGenerator
    {
        Task<string> GenerateAsync(string specification, string @namespace, CancellationToken cancellationToken = default);
    }
}
