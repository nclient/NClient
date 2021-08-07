using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NClient.AspNetCore.Exceptions;
using NClient.Sandbox.FileService.Facade;

#pragma warning disable 1998

namespace NClient.Sandbox.FileService.Controllers
{
    [ApiController, Route("api/ignored/[controller]")] // Must be ignored
    public class FileController : ControllerBase, IFileController
    {
        private readonly ILogger<FileController> _logger;

        public FileController(ILogger<FileController> logger)
        {
            _logger = logger;
        }
        
        public async Task<IActionResult> GetTextFileAsync(long id)
        {
            _logger.LogInformation($"File with an id '{id}'was requested.");
            
            if (id < 0)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            if (id == 0)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return PhysicalFile(Path.GetFullPath("Files/TextFile.txt"), "text/plain");
        }

        public Task PostTextFileAsync(byte[] fileBytes)
        {
            if (fileBytes.Length == 0)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            
            _logger.LogInformation($"File was saved (not really).");
            return Task.CompletedTask;
        }
        
        public async Task<IActionResult> GetImageAsync(long id)
        {
            _logger.LogInformation($"File with an id '{id}' was requested.");
            
            if (id < 0)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            if (id == 0)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            
            return PhysicalFile(Path.GetFullPath("Files/Image.jpg"), "image/jpeg");
        }
        
        public Task PostImageFileAsync(byte[] fileBytes)
        {
            if (fileBytes.Length == 0)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            
            _logger.LogInformation($"File was saved (not really).");
            return Task.CompletedTask;
        }
    }
}
