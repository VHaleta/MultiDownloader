using Microsoft.AspNetCore.Mvc;
using MultiDownloader.DownloaderApi.Host.Interfaces;
using MultiDownloader.DownloaderApi.Host.Models;

namespace MultiDownloader.DownloaderApi.Host.Controllers
{
    [ApiController]
    [Route("api/downloader")]
    public class DownloaderController : ControllerBase
    {
        private readonly ILogger<DownloaderController> _logger;
        private readonly IDownloaderProcessor _downloaderProcessor;

        public DownloaderController(ILogger<DownloaderController> logger, IDownloaderProcessor downloaderProcessor)
        {
            _logger = logger;
            _downloaderProcessor = downloaderProcessor;
        }

        [HttpGet("formats")]
        public async Task<IActionResult> GetAvailableFormats([FromBody] FormatsRequestPayload requestPayload)
        {
            _logger.LogInformation("Begin getting available formats for URL: " + requestPayload.URL);

            List<string> formats = (await _downloaderProcessor
                .GetAvailableFormatsAsync(requestPayload))
                .ToList();

            return Ok(new FormatsResponcePayload() { Formats = formats });
        }

        [HttpPost("download")]
        public async Task<IActionResult> DownloadFile([FromBody] FileRequest request)
        {
            _logger.LogInformation("Запрос на скачивание файла для формата: {Format}", request.Format);

            // Здесь мог бы быть код, который генерирует файл на основе запроса
            // В этом примере возвращается статический текстовый файл

            var content = "Это содержимое файла.";
            var fileName = "example.txt";

            var filePath = Path.GetTempFileName();
            await System.IO.File.WriteAllTextAsync(filePath, content);

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var result = File(stream, "application/octet-stream", fileName);

            return result;
        }
    }
    public class FileRequest
    {
        public string Format { get; set; }
        public Dictionary<string, string> AdditionalFields { get; set; }
    }
}
