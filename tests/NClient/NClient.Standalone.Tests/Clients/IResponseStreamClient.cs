using System.IO;
using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Providers.Transport;
using NClient.Testing.Common.Clients;

namespace NClient.Standalone.Tests.Clients
{
    [Path("api/responseStream")]
    public interface IResponseStreamClientWithMetaData : IResponseStreamClient
    {
        [GetMethod]
        new Task<IResponse> GetResponseAsync();
        
        [GetMethod]
        new Task<IResponse<int>> GetResponseWithDataAsync();
        
        [GetMethod]
        new Task<IResponseWithError<string>> GetResponseWithErrorAsync();
        
        [GetMethod]
        new Task<IResponseWithError<int, string>> GetResponseWithDataOrErrorAsync();
        
        [GetMethod]
        new Task<Stream> GetStreamAsync();
    }
}
