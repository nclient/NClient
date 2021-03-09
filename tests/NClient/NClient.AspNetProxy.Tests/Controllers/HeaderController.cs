using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NClient.Testing.Common.Clients;

namespace NClient.AspNetProxy.Tests.Controllers
{
    [ApiController]
    [Route("api/header")]
    public class HeaderController : ControllerBase, IHeaderClient
    {
        [HttpGet]
        public Task<int> GetAsync([FromHeader] int id) => Task.FromResult(1);

        [HttpDelete]
        public Task DeleteAsync([FromHeader] int id) => Task.FromResult(0);
    }
}
