using Awesome.Services.FileUploadService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Awesome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController(IFileUploadService fileUploadService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UploadFileAsync([FromForm] IFormFile? file)
        {
            if (file == null)
            {
                return BadRequest("No file was uploaded");
            }
            const string folder = "uploads";
            var fileName = file.FileName;
            var stream = file.OpenReadStream();
            
            var url = await fileUploadService.UploadFileAsync(folder, fileName, stream);
            
            return Ok(new { url });
        }
    }
}
