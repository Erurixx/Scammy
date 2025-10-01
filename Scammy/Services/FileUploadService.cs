using System.Text;
using System.Text.Json;

namespace Scammy.Services
{
    /// <summary>
    /// Service to handle file uploads to AWS Lambda via API Gateway
    /// </summary>
    public class FileUploadService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<FileUploadService> _logger;

        public FileUploadService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<FileUploadService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Upload a file to AWS S3 via Lambda microservice
        /// </summary>
        public async Task<FileUploadResponse> UploadFileAsync(IFormFile file)
        {
            try
            {
                _logger.LogInformation($"Starting upload for file: {file.FileName}");

                // Validate file
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("File is empty or null");
                }

                // Read file content into memory
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                _logger.LogInformation($"File size: {fileBytes.Length} bytes");

                // Convert to base64 for JSON transmission
                var base64Content = Convert.ToBase64String(fileBytes);

                // Get API Gateway URL from configuration
                var apiGatewayUrl = _configuration["AWS:ApiGatewayUrl"];

                if (string.IsNullOrEmpty(apiGatewayUrl))
                {
                    throw new InvalidOperationException(
                        "AWS:ApiGatewayUrl not configured in appsettings.json");
                }

                _logger.LogInformation($"Sending to API Gateway: {apiGatewayUrl}");

                // Prepare request payload
                var requestData = new
                {
                    fileContent = base64Content,
                    fileName = file.FileName,
                    fileType = file.ContentType ?? "application/octet-stream"
                };

                // Serialize to JSON
                var jsonContent = JsonSerializer.Serialize(requestData);
                var httpContent = new StringContent(
                    jsonContent,
                    Encoding.UTF8,
                    "application/json");

                // Send POST request to API Gateway
                var response = await _httpClient.PostAsync(apiGatewayUrl, httpContent);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation($"Response status: {response.StatusCode}");
                _logger.LogInformation($"Response content: {responseContent}");

                // Parse response
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var result = JsonSerializer.Deserialize<FileUploadResponse>(
                        responseContent,
                        options);

                    if (result == null)
                    {
                        return new FileUploadResponse
                        {
                            Success = true,
                            Message = "Upload completed but no response data received"
                        };
                    }

                    _logger.LogInformation($"Upload successful: {result.FileName}");
                    return result;
                }
                else
                {
                    _logger.LogError($"Upload failed: {responseContent}");
                    throw new Exception($"Upload failed with status {response.StatusCode}: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during file upload");
                throw;
            }
        }
    }

    /// <summary>
    /// Response model from Lambda function
    /// </summary>
    public class FileUploadResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string S3Key { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string UploadTime { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
}