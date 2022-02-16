using System.Threading;
using System.Threading.Tasks;

namespace NClient.CodeGeneration.Abstractions
{
    public interface INClientFacadeGenerator
    {
        Task<string> GenerateAsync(string specification, FacadeGenerationSettings generationSettings, CancellationToken cancellationToken = default);
    }
}
