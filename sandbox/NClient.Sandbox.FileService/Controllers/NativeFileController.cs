using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NClient.AspNetCore.Exceptions;

#pragma warning disable 1998

namespace NClient.Sandbox.FileService.Controllers
{
    [ApiController]
    [ApiVersion("1.0"), ApiVersion("2.0"), ApiVersion("3.0")]
    [Route("api/native/v{version:apiVersion}/weatherForecast")]
    public class NativeFileController : ControllerBase
    {
        private readonly ILogger<NativeFileController> _logger;

        public NativeFileController(ILogger<NativeFileController> logger)
        {
            _logger = logger;
        }

        [HttpGet("bytes/{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetFileBytesAsync([FromRoute] long id)
        {
            _logger.LogInformation($"File with an id '{id}'was requested.");
            
            if (id < 0)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            if (id == 0)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return PhysicalFile(Path.GetFullPath("Files/TextFile.txt"), "text/plain");
        }

        [HttpPost("bytes")]
        [ProducesResponseType(typeof(void), 400)]
        public Task PostFileBytesAsync(byte[] fileBytes)
        {
            if (fileBytes.Length == 0)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            
            _logger.LogInformation($"File was saved (not really).");
            return Task.CompletedTask;
        }
        
        [HttpGet("files/{id}")]
        [ProducesResponseType(typeof(byte[]), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetFileAsync([FromRoute] long id)
        {
            _logger.LogInformation($"File with an id '{id}' was requested.");
            
            if (id < 0)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            if (id == 0)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            
            return PhysicalFile(Path.GetFullPath("Files/Image.jpg"), "image/jpeg");
        }
    }
}
