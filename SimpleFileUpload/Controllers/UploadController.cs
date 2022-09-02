using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SimpleFileUpload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {

        // POST: api/Solve
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var uploadPath = Path.Combine("uploads", file.FileName);

            // Saving File on Server
            if (file.Length > 0)
            {
                await using var fileStream = new FileStream(uploadPath, FileMode.Create);
                await file.CopyToAsync(fileStream);
            }
            else
            {
                BadRequest("The file you are trying to upload has a lengh of zero.");
            }

            return Ok("Your upload completed successfully!");
        }
    }
}
