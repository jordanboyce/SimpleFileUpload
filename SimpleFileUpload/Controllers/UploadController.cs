using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace SimpleFileUpload.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UploadController : ControllerBase
    {

        // Get: api/ListFiles
        [HttpGet]
        [Route("listfiles")]
        public async Task<IActionResult> ListFiles(string directory = "uploads")
        {
            DirectoryInfo d = new DirectoryInfo(directory);

            FileInfo[] Files = d.GetFiles();
            List<string> fileList = new List<string>();

            foreach (FileInfo file in Files)
            {
                fileList.Add(file.Name);
            }

            return Ok(fileList);
        }

        // POST: api/UploadFile
        [HttpPost]
        [Route("uploadfile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {

            if (file.Length < 0)
            {
                return BadRequest("File cannot be empty");
            }

            var uploadPath = Path.Combine("uploads", file.FileName);

            // Saving File on Server
            if (!System.IO.File.Exists(uploadPath))
            {
                await using var fileStream = new FileStream(uploadPath, FileMode.Create);
                await file.CopyToAsync(fileStream);
            }
            else
            {
                return BadRequest("A file with the same name already exists.");
            }

            return Ok("Your upload completed successfully!");
        }

        // Get: api/ReadFile
        [HttpGet]
        [Route("readfile")]
        public async Task<IActionResult> ReadFile(string filename)
        {
            var pathToFile = Path.Combine("uploads", filename);

            // Check if file exists and read
            if (System.IO.File.Exists(pathToFile))
            {
                string readText = await System.IO.File.ReadAllTextAsync(pathToFile);

                return Ok(readText);
            }
            else
            {
                return NotFound("File not found.");
            }
        }

        // Get: api/DeleteFile
        [HttpDelete]
        [Route("deletefile")]
        public async Task<IActionResult> DeleteFile(string filename)
        {
            var pathToFile = Path.Combine("uploads", filename);

            // Check if file exists and read
            if (System.IO.File.Exists(pathToFile))
            {
                System.IO.File.Delete(pathToFile);
                return Ok("File deleted successfully.");
            }
            else
            {
                return NotFound("File Not Found");
            }
        }

    }
}
