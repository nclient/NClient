using System.Threading;
using System.Threading.Tasks;

namespace NClient.CodeGeneration.Abstractions
{
    public interface INClientFacadeGenerator
    {
        Task<string> GenerateAsync(string specification, string @namespace, string facadeName, CancellationToken cancellationToken = default);
    }
}
