using BlobTestApp.Models;
using BlobTestApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlobTestApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlobStorageService _blobStorageService;

        public HomeController(ILogger<HomeController> logger, IBlobStorageService blobStorageService)
        {
            _logger = logger;
            _blobStorageService = blobStorageService;
        }

        public async Task<IActionResult> Index()
        {
            var imageUrls=await _blobStorageService.ListFilesAsync();
            return View(imageUrls);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty");
            }

            using var stream = file.OpenReadStream();
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var fileUrl = await _blobStorageService.UploadAsync(stream, fileName);
            return RedirectToAction("Index");
        }

        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var stream = await _blobStorageService.DownloadAsync(fileName);
            var contentType = GetContentType(fileName);
            return File(stream, contentType, fileName);
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".mp4" => "video/mp4",
                _ => "application/octet-stream"
            };
        }

    }
}
