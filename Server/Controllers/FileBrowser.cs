using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileBrowserController : ControllerBase
    {

        [HttpGet("files")]
        public IActionResult GetFiles(string path = "") // Optional subpath
        {
            //string fullPath = Path.Combine(_rootDirectory, path);
            string fullPath = path;

            if (!Directory.Exists(fullPath))
            {
                return NotFound(); // Or appropriate error handling
            }

            var files = Directory.GetFiles(fullPath)
                           .Select(f => new { Name = Path.GetFileName(f), Path = Path.Combine(fullPath, Path.GetFileName(f)).Replace("\\", "/") }) // Relative path
                           .ToList();

            var directories = Directory.GetDirectories(fullPath)
                                .Select(d => new { Name = Path.GetFileName(d), Path = Path.Combine(fullPath, Path.GetFileName(d)).Replace("\\", "/") }) // Relative path
                                .ToList();

            return Ok(new { Files = files, Directories = directories });
        }

        [HttpGet("download")]
        public IActionResult DownloadFile(string path)
        {
            // ... (Get the full file path from the relative path) ...
            string fullPath = path;

            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound();
            }

            var bytes = System.IO.File.ReadAllBytes(fullPath);
            return File(bytes, "application/octet-stream", Path.GetFileName(fullPath)); // Force download
        }

        // For serving the file content (if needed):
        [HttpGet("file-content")]
        public IActionResult GetFileContent(string path)
        {
            string fullPath = path;

            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound();
            }

            try
            {
                var bytes = System.IO.File.ReadAllBytes(fullPath);
                return File(bytes, GetContentType(path), Path.GetFileName(path)); // Set correct content type
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Handle errors!
            }
        }

        private string GetContentType(string path)
        {
            // Implement logic to determine content type (MIME type) based on file extension.
            // Example (improve with a more robust mapping):
            string ext = Path.GetExtension(path).ToLower();
            switch (ext)
            {
                case ".txt": return "text/plain";
                case ".pdf": return "application/pdf";
                case ".jpg": case ".jpeg": return "image/jpeg";
                // ... more types
                default: return "application/octet-stream"; // Default for unknown types
            }
        }
    }
}
