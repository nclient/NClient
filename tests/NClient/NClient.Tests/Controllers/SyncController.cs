using Microsoft.AspNetCore.Mvc;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;

namespace NClient.Tests.Controllers
{
    [ApiController]
    [Route("api/sync")]
    public class SyncController : ControllerBase, ISyncClient
    {
        [HttpGet]
        public int Get(int id) => 1;

        [HttpPost]
        public void Post(BasicEntity entity) { }

        [HttpPut]
        public void Put(BasicEntity entity) { }

        [HttpDelete]
        public void Delete(int id) { }
    }
}
