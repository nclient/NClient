using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.Tests.Controllers
{
    [ApiController]
    [Route("api/query")]
    public class QueryController : ControllerBase, IQueryClient
    {
        [HttpGet]
        public Task<int> GetAsync(int id) => Task.FromResult(1);

        [HttpPost]
        public Task PostAsync([FromQuery] BasicEntity entity) => Task.FromResult(0);

        [HttpPut]
        public Task PutAsync([FromQuery] BasicEntity entity) => Task.FromResult(0);

        [HttpDelete]
        public Task DeleteAsync(int id) => Task.FromResult(0);
    }
}
