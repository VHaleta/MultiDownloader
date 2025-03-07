using Microsoft.AspNetCore.Mvc;
using MultiDownloader.DownloaderApi.DownloaderProvider;
using MultiDownloader.DownloaderApi.DownloaderProvider.Models;
using MultiDownloader.DownloaderApi.Host.Models;
using System.Text.Json;

namespace MultiDownloader.DownloaderApi.Host.Controllers
{
    [ApiController]
    [Route("api/downloader")]
    public class DownloaderController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IDownloader _downloaderProcessor;

        public DownloaderController(
            Serilog.ILogger logger,
            IDownloader downloaderProcessor)
        {
            _logger = logger;
            _downloaderProcessor = downloaderProcessor;
        }

        [HttpGet("formats")]
        public async Task<string> GetAvailableFormats([FromQuery] string url)
        {
            _logger.Information("Begin getting available formats for URL: " + url);

            try
            {
                List<FormatInfo> formats = (await _downloaderProcessor
                    .GetAvailableFormatsAsync(url))
                .ToList();

                string json = JsonSerializer.Serialize(new FormatsResponcePayload() { Formats = formats });

                return json;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                string json = JsonSerializer.Serialize(new FormatsResponcePayload() { Error = "Error" });
                return json;
            }
        }

        [HttpPost("download")]
        public async Task<IActionResult> DownloadFile([FromBody] FileDownloadRequestPayload request)
        {
            _logger.Information("Begin downloading file for URL: " + request.URL);

            FileData fileData = await _downloaderProcessor
                .DownloadFileAsync(request.URL, request.Format, request.Resolution);

            _logger.Information($"{fileData.Path}: downloaded");
            return Ok(
                new FileDownloadResponcePayload()
                { FileData = new FileData { Name = fileData.Name, Path = fileData.Path } });
        }
    }
}
