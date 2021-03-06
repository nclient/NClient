using System.Net;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.Tests.Clients
{
    [Path("api/response")]
    public interface IResponseClientWithMetadata : IResponseClient
    {
        [GetMethod]
        [Response(typeof(int), HttpStatusCode.OK)]
        [Response(typeof(string), HttpStatusCode.BadRequest)]
        [Response(typeof(void), HttpStatusCode.InternalServerError)]
        new Task<int> GetAsync(int id);

        [GetMethod]
        [Response(typeof(int), HttpStatusCode.OK)]
        [Response(typeof(string), HttpStatusCode.BadRequest)]
        [Response(typeof(void), HttpStatusCode.InternalServerError)]
        new Task<HttpResponse<int>> GetResponseAsync(int id);

        [GetMethod]
        [Response(typeof(int), HttpStatusCode.OK)]
        [Response(typeof(string), HttpStatusCode.BadRequest)]
        [Response(typeof(void), HttpStatusCode.InternalServerError)]
        new Task<HttpResponseWithError<int, Error>> GetResponseWithErrorAsync(int id);

        [PostMethod]
        [Response(typeof(void), HttpStatusCode.OK)]
        [Response(typeof(string), HttpStatusCode.BadRequest)]
        [Response(typeof(void), HttpStatusCode.InternalServerError)]
        new Task PostAsync(BasicEntity entity);

        [PostMethod]
        [Response(typeof(void), HttpStatusCode.OK)]
        [Response(typeof(string), HttpStatusCode.BadRequest)]
        [Response(typeof(void), HttpStatusCode.InternalServerError)]
        new Task<HttpResponse> PostResponseAsync(BasicEntity entity);

        [PostMethod]
        [Response(typeof(void), HttpStatusCode.OK)]
        [Response(typeof(string), HttpStatusCode.BadRequest)]
        [Response(typeof(void), HttpStatusCode.InternalServerError)]
        new Task<HttpResponseWithError<Error>> PostResponseWithErrorAsync(BasicEntity entity);
    }
}
