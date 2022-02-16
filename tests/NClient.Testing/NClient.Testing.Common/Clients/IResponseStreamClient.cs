using System.IO;
using System.Threading.Tasks;
using NClient.Providers.Transport;

namespace NClient.Testing.Common.Clients
{
    public interface IResponseStreamClient
    {
        Task<IResponse> GetResponseAsync();
        Task<IResponse<int>> GetResponseWithDataAsync();
        Task<IResponseWithError<string>> GetResponseWithErrorAsync();
        Task<IResponseWithError<int, string>> GetResponseWithDataOrErrorAsync();
        Task<Stream> GetStreamAsync();
    }
}
