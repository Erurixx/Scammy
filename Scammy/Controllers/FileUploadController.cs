using Microsoft.AspNetCore.Mvc;
using Scammy.Services;

namespace Scammy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly FileUploadService _fileUploadService;
        private readonly ILogger<FileUploadController> _logger;

        public FileUploadController(
            FileUploadService fileUploadService,
            ILogger<FileUploadController> logger)
        {
            _fileUploadService = fileUploadService;
            _logger = logger;
        }

        /// <summary>
        /// Upload file to AWS S3 via Lambda microservice
        /// POST: api/FileUpload
        /// </summary>
        [HttpPost]
        [RequestSizeLimit(10_000_000)] // 10MB limit
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                _logger.LogInformation("File upload request received");

                // Validate file
                if (file == null || file.Length == 0)
                {
                    _logger.LogWarning("No file provided in request");
                    return BadRequest(new
                    {
                        success = false,
                        error = "No file provided",
                        message = "Please select a file to upload"
                    });
                }

                // Check file size (10MB max)
                if (file.Length > 10_000_000)
                {
                    _logger.LogWarning($"File too large: {file.Length} bytes");
                    return BadRequest(new
                    {
                        success = false,
                        error = "File too large",
                        message = "Maximum file size is 10MB"
                    });
                }

                _logger.LogInformation($"Processing file: {file.FileName}, Size: {file.Length} bytes");

                // Upload via service
                var result = await _fileUploadService.UploadFileAsync(file);

                _logger.LogInformation($"File uploaded successfully: {result.FileName}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file");
                return StatusCode(500, new
                {
                    success = false,
                    error = "Upload failed",
                    message = ex.Message
                });
            }
        }

        /// <summary>
        /// Health check endpoint
        /// GET: api/FileUpload/health
        /// </summary>
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new
            {
                service = "Scammy File Upload",
                status = "healthy",
                timestamp = DateTime.UtcNow
            });
        }
    }
}