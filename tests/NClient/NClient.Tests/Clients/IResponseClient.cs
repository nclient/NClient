using System.Net;
using System.Threading.Tasks;
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
        [Response(typeof(void), HttpStatusCode.BadRequest)]
        new Task<int> GetAsync(int id);

        [PostMethod]
        [Response(typeof(void), HttpStatusCode.OK)]
        [Response(typeof(void), HttpStatusCode.BadRequest)]
        new Task PostAsync(BasicEntity entity);
    }
}
