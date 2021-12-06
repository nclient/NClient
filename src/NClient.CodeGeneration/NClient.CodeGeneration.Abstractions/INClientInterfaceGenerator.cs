using System.Threading;
using System.Threading.Tasks;

namespace NClient.CodeGeneration.Abstractions
{
    public interface INClientInterfaceGenerator
    {
        Task<string> GenerateAsync(string specification, string @namespace, string interfaceName, CancellationToken cancellationToken = default);
    }
}
