using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.Tests.Controllers
{
    [ApiController]
    [Route("api")]
    public class ReturnController : ControllerBase, IReturnClient
    {
        [HttpGet]
        public Task<BasicEntity> GetAsync(int id) => Task.FromResult(new BasicEntity { Id = 1, Value = 2 });

        [HttpGet]
        public BasicEntity Get(int id) => new BasicEntity { Id = 1, Value = 2 };

        [HttpPost]
        public Task PostAsync(BasicEntity entity) => Task.FromResult(0);

        [HttpPost]
        public void Post(BasicEntity entity) { }
    }
}
