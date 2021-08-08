using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Annotations;
using NClient.Annotations.Parameters;
using NClient.Annotations.Versioning;

namespace NClient.Sandbox.FileService.Facade
{
    [UseVersion("3.0")]
    [Header("client", "NClient")]
    public interface IFileClient : IFileController
    {
        [Override]
        new Task<HttpResponse> GetTextFileAsync([RouteParam] long id);

        [Override]
        new Task<HttpResponse> GetImageAsync(long id);
    }
}
