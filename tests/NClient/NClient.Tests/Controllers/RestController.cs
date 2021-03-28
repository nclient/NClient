using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.Tests.Controllers
{
    [ApiController]
    [Route("api/rest")]
    public class RestController : ControllerBase, IRestClient
    {
        [HttpGet("{id}")]
        public Task<int> GetAsync(int id) => Task.FromResult(1);

        [HttpPost]
        public Task PostAsync(BasicEntity entity) => Task.FromResult(0);

        [HttpPut]
        public Task PutAsync(BasicEntity entity) => Task.FromResult(0);

        [HttpDelete("{id}")]
        public Task DeleteAsync(int id) => Task.FromResult(0);
    }
}
