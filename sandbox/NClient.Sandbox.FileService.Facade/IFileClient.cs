using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Providers.Transport;

namespace NClient.Sandbox.FileService.Facade
{
    [UseVersion("3.0")]
    [Header("client", "NClient")]
    public interface IFileClient : IFileController
    {
        [Override]
        new Task<IResponse> GetTextFileAsync([RouteParam] long id);

        [Override]
        new Task<IResponse> GetImageAsync(long id);
    }
}
