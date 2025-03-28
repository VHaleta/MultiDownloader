using MultiDownloader.DownloaderApi.DownloaderProvider.Models;

namespace MultiDownloader.DownloaderApi.Host.Models
{
    public class FileDownloadResponcePayload
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string? Error { get; set; }
    }
}
